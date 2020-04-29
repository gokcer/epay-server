using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Threading;
using Epay3.Api.Models;
using Epay3.Api.Models.Api;
using Epay3.Api.Services;
using Epay3.Api.Tenancy;
using Epay3.Common;
using log4net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Epay3.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/mobile")]
    [EnableCors("MyPolicy")]
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public class MobileController : Controller
    {
        private readonly AppTenant _appTenant;
        private readonly MessagingService _messagingService;
        private readonly OneSignalService _oneSignalService;
        private readonly Epay3Context _db;

        private bool bypassChecks = false;

        private const string SPECIAL_CODE_DEPOSIT = "DEPOSIT";


        ILog logger = LogManager.GetLogger(typeof(MobileController));

        private bool setCancelTransaction = false;

        public MobileController(Epay3Context db, AppTenant appTenant, MessagingService messagingService,
            OneSignalService oneSignalService)
        {
            _db = db;
            _appTenant = appTenant;
            _messagingService = messagingService;
            _oneSignalService = oneSignalService;
        }

        /**
         * Tries to register new device with device serial and registration token
         */
        [HttpPost]
        [Route("register")]
        public RestResult<RegisterResponse> RegisterDevice([FromBody] RegisterRequest registerRequest)
        {
            var site = _db.Site.First();

            // check site registration code
            if (registerRequest.Token != site.RegistrationCode)
            {
                Thread.Sleep(5);
                return new RestResult<RegisterResponse>(false, "", null);
            }

            // does device exists?
            var device = _db.Device.FirstOrDefault(d => d.Serial == registerRequest.Serial);
            if (device == null)
            {
                // no. Create a new one with unique device token
                device = new Device() {DeviceToken = Guid.NewGuid().ToString(), Serial = registerRequest.Serial};
                _db.Device.Add(device);
                _db.SaveChanges();
            }

            // todo optional source address, date etc. can be recorded
            // return device token
            var registerResponse = new RegisterResponse() {Slot = site.Slot.Value, DeviceToken = device.DeviceToken};
            return new RestResult<RegisterResponse>(registerResponse);
        }

        [HttpPost]
        [Route("getCard")]
        public RestResult<CardResponse> GetCard([FromHeader] string authToken, [FromBody] GetCardRequest getCardRequest,
            [FromHeader] string productType)
        {
            if (!bypassChecks)
            {
                var deviceLogin = _db.DeviceLogin.First(dl => dl.Token == authToken);
            }

            var iProductType = int.Parse(productType);

            var card = _db.Card.FirstOrDefault(c => c.CardNo == getCardRequest.CardNo);

            if (card == null)
            {
                return new RestResult<CardResponse>(false, "Card not found", null);
            }
            else
            {
                // calculate current balance
                var siteNow = getSiteNow();
                var transactionDetails = _db.TransactionDetail.Where(td =>
                        td.TransactionNavigation.Card == card.Oid
                        && td.TransactionNavigation.ProductType == iProductType
                        && (td.ValidFrom == null || td.ValidFrom <= siteNow)
                        && (td.ValidTo == null || td.ValidTo >= siteNow)
                    )
                    .ToList();
                var balance = transactionDetails.Sum(td => td.Total);

                return new RestResult<CardResponse>(true, null, new CardResponse()
                {
                    CardNo = card.CardNo,
                    Balance = balance ?? 0,
                    Name = card.CustomerNavigation?.Name,
                    UserName = card.CustomerNavigation?.UserName,
                    MinimumBalanceLimit = card.MinimumBalanceLimit
                });
            }
        }

        [HttpPost]
        [Route("createCard")]
        public RestResult<CardResponse> CreateCard([FromHeader] string authToken,
            [FromBody] CreateCardRequest createCardRequest)
        {
            var deviceLogin = _db.DeviceLogin.First(dl => dl.Token == authToken);
            var card = _db.Card.Include(c => c.Transaction).FirstOrDefault(c => c.CardNo == createCardRequest.CardNo);

            if (card != null)
            {
                return new RestResult<CardResponse>(false, "Card exists", null);
            }
            else
            {
                // hack. Correct number for Turkey.
                var phoneNumbers = createCardRequest.Phone.Where(c => char.IsDigit(c)).TakeLast(10).ToArray();
                createCardRequest.Phone = "+90" + new string(phoneNumbers);

                // Create new customer
                var newCustomer = new Customer();
                newCustomer.CitizenshipNumber = createCardRequest.CitizenshipNumber;
                newCustomer.Name = createCardRequest.Name;
                newCustomer.Phone = createCardRequest.Phone;
                newCustomer.Token = Guid.NewGuid().ToString();

                // Create new card
                var newCard = new Card();
                newCard.CardNo = createCardRequest.CardNo;
                newCard.Active = true;
                newCard.Returnable = createCardRequest.Returnable;

                // Link them
                newCard.CustomerNavigation = newCustomer;

                // Persist new items
                _db.Card.Add(newCard);

                _db.SaveChanges();

                // Retrieve card from db
                var cardFromDb = _db.Card.Include(c => c.Transaction)
                    .FirstOrDefault(c => c.CardNo == createCardRequest.CardNo);

                return new RestResult<CardResponse>(true, null, new CardResponse()
                {
                    CardNo = cardFromDb.CardNo,
                    Balance = cardFromDb.Transaction.Sum(t => t.Amount) ?? 0,
                    Name = cardFromDb.CustomerNavigation?.Name,
                    UserName = cardFromDb.CustomerNavigation?.UserName
                });
            }
        }


        [HttpPost]
        [Route("login")]
        public RestResult<LoginResponse> Login([FromHeader] string deviceToken, [FromBody] LoginRequest loginRequest)
        {
            var device = _db.Device.First(d => d.DeviceToken == deviceToken);
            Customer employee = null;

            //
            // try login 
            //
            employee = _db.Customer.FirstOrDefault
            (
                e => e.Active == true && e.UserName == loginRequest.UserName &&
                     (
                         (e.Password == loginRequest.Password) ||
                         (e.Card.Any(c => c.Active == true && c.CardNo == loginRequest.Password))
                     )
            );

            if (employee != null)
            {
                // generate new authToken
                var authToken = Guid.NewGuid().ToString();

                _db.DeviceLogin.Add(new DeviceLogin()
                {
                    Date = getSiteNow(),
                    DeviceNavigation = device,
                    EmployeeNavigation = employee,
                    UserNameReceived = loginRequest.UserName,
                    PasswordReceived = loginRequest.Password,
                    Token = authToken
                });

                _db.SaveChanges();

                var productType = device.ProductType ?? (int)EProductType.Money;

                var query = _db.TransactionItem
                        .Include(i => i.CategoryNavigation)
                        .Where(i => (bool) i.Active && i.ProductType == productType)
                    ;

                if (!(employee.CanCharge.HasValue && employee.CanCharge.Value))
                {
                    query = query.Where(i => (i.Type != (int)ETransactionType.Charge)); 
                }

                var transactionItemResponses = query
                    .OrderBy(i => i.CategoryNavigation.Order)
                    .ThenByDescending(i => i.CategoryNavigation.Name)
                    .ThenBy(i => i.Name)
                    .Select(i =>
                        new TransactionItemResponse()
                        {
                            IdTransactionItem = i.Oid,
                            Name = i.Name,
                            Price = (decimal) i.Price,
                            ProductType =
                                (int) i.Type,
                            Category = i.CategoryNavigation.Name
                        }).ToList();

                var allowedOrderStatuses = new List<ConfigurationOrderStatus>();

                if (employee.CanResetOrder == true)
                    allowedOrderStatuses.Add(new ConfigurationOrderStatus("Reset", (int) EOrderStatus.New));
                if (employee.CanStartOrder == true)
                    allowedOrderStatuses.Add(new ConfigurationOrderStatus("Start", (int) EOrderStatus.InProgress));
                if (employee.CanCancelOrder == true)
                    allowedOrderStatuses.Add(new ConfigurationOrderStatus("Cancel", (int) EOrderStatus.Canceled));
                if (employee.CanCompleteOrder == true)
                    allowedOrderStatuses.Add(new ConfigurationOrderStatus("Complete", (int) EOrderStatus.Completed));

                // create new login and return auth token
                return new RestResult<LoginResponse>(new LoginResponse()
                {
                    IdEmployee = employee.Oid,
                    AuthToken = authToken,
                    Configuration = new Configuration()
                    {
                        MinimumBalanceLimit = 10,
                        TransactionItems = transactionItemResponses,
                        DeviceMode = device.DeviceMode ?? 0,
                        ProductType = productType,
                        CanCharge = employee.CanCharge,
                        CanSale = employee.CanSale,
                        CanManageTable = employee.CanManageTable,
                        AllowedOrderStatuses = allowedOrderStatuses,
                        NotificationToken = employee.NotificationToken,
                        PollPeriod = device.PollPeriod
                    }
                });
            }
            else
            {
                return new RestResult<LoginResponse>(false, "Login failed", null);
            }
        }

        [HttpPost]
        [Route("dashlogin")]
        public RestResult<LoginResponse> DashboardLogin([FromBody] LoginRequest loginRequest)
        {
            Customer employee = null;

            // try login 
            //
            employee = _db.Customer.FirstOrDefault
            (
                e => e.Active == true && e.UserName == loginRequest.UserName &&
                     (
                         (e.Password == loginRequest.Password) ||
                         (e.Card.Any(c => c.Active == true && c.CardNo == loginRequest.Password))
                     )
            );
            if (employee != null)
            {
                var panelPredefinedSerial = "PANEL";
                var devicePanel = _db.Device.FirstOrDefault(d => d.Serial == panelPredefinedSerial);
                if (devicePanel == null)
                {
                    devicePanel = new Device() {Serial = panelPredefinedSerial};
                    _db.Device.Add(devicePanel);
                }

                var deviceLogin = new DeviceLogin()
                {
                    Date = DateTime.Now, UserNameReceived = loginRequest.UserName, EmployeeNavigation = employee,
                    PasswordReceived = loginRequest.Password, Success = "true", DeviceNavigation = devicePanel,
                    Token = Guid.NewGuid().ToString()
                };
                _db.DeviceLogin.Add(deviceLogin);
                _db.SaveChanges();
                return new RestResult<LoginResponse>(new LoginResponse()
                {
                    IdEmployee = employee.Oid,
                    AuthToken = deviceLogin.Token
                });
            }
            else

            {
                return new RestResult<LoginResponse>(false, "Login failed", null);
            }
        }

        [HttpPost]
        [Route("customer/login")]
        public RestResult<CustomerLoginResponse> CustomerLogin([FromHeader] string customerToken)
        {
            var siteNow = getSiteNow();

            var transactionItemResponses = _db.TransactionItem
                .Where(i => (bool) i.Active && i.Price != 0 && i.ProductType == 0 && i.StationItem.Any()).Select(i =>
                    new TransactionItemResponse()
                    {
                        IdTransactionItem = i.Oid,
                        Name = i.Name,
                        Price = (decimal) i.Price,
                        Category = i.CategoryNavigation.Name,
                        Icon = i.CategoryNavigation.Icon,
                        Image = i.Image,
                        ProductType =
                            1 // todo important product type must be refactored (renamed) to type!!! After multi product type development, Product Type of TransactionItem refers to currency type!
                    }).ToList();

            var customerLoginResponse = new CustomerLoginResponse();

            var configuration = new Configuration();
            configuration.TransactionItems = transactionItemResponses;

            customerLoginResponse.Configuration = configuration;

            configuration.Clients = new List<ConfigurationClient>();
            configuration.Clients.Add(new ConfigurationClient() {Name = "Epay", Code = "epay"});

            var customerReport = CustomerReport(customerToken);
            if (customerReport.Success == false)
            {
                return new RestResult<CustomerLoginResponse>(false, "Login failed! " + customerReport.Message,
                    null);
            }

            customerLoginResponse.CustomerReport = customerReport.Data;

            logger.Debug(new {message="Customer login for "+customerReport.Data.CustomerKey,report = customerReport});

            return new RestResult<CustomerLoginResponse>(customerLoginResponse);
        }

        [HttpPost]
        [Route("saveOnlineTransaction")]
        public RestResult<SaveTransactionResponse> SaveOnlineTransaction([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromHeader] string productType,
            [FromBody] SaveOnlineTransactionRequest request)
        {
            int iProductType = int.Parse(productType);

            logger.Debug($"(Client={_appTenant.Client}) Saving transaction: {JsonConvert.SerializeObject(request)}");
            var sourceAddress = Request.Host.Value;
            int errorCode = 0;
            try
            {
                errorCode = 100;
                var deviceLogin = bypassChecks ? null : DeviceLogin(authToken, deviceToken);

                errorCode = 101;
                // find and verify employee from Auth token, transaction token, imei and employee id
                // auth token can be different from transaction token if transaction was saved and new login done before sending tx.

                errorCode = 102;
                //

                errorCode = 103;
                var card = _db.Card.Include(c => c.CustomerNavigation).First(c => c.CardNo == request.CardNo);

                errorCode = 104;
                //
                // create transaction
                Site site = getSite();
                var siteNow = getSiteNow();

                // get existing balance fron transaction items
                var existingTransactionDetails = _db
                    .TransactionDetail
                    .Include(td => td.ItemNavigation)
                    .Where(td =>
                        td.TransactionNavigation.CardNavigation == card &&
                        td.ItemNavigation.ProductType == iProductType
                        && (td.ValidFrom == null || td.ValidFrom <= siteNow)
                        && (td.ValidTo == null || td.ValidTo >= siteNow)
                    ).ToList();
                var cardBalance = existingTransactionDetails.Any()
                    ? existingTransactionDetails.Sum(t => t.Total)
                    : 0;

                errorCode = 105;

                bool canCharge = false;
                if (bypassChecks)
                {
                    canCharge = false;
                }
                else
                {
                    var employeeNavigationCanCharge = deviceLogin.EmployeeNavigation.CanCharge;
                    canCharge = employeeNavigationCanCharge.HasValue && employeeNavigationCanCharge.Value;
                }

                List<TransactionDetail> transactionDetails = new List<TransactionDetail>();

                var idItemsAtOrder = request.OrderItems.Select(oi => oi.IdTransactionItem).ToList();

                foreach (var orderItem in request.OrderItems)
                {
                    var transactionItem = _db.TransactionItem.Find(orderItem.IdTransactionItem);

                    if (transactionItem.ProductType != iProductType)
                    {
                        // security exception
                        throw new SecurityException(AppLocalization
                            .MobileController_SaveOnlineTransaction_Invalid_order_item);
                    }

                    if (!canCharge && transactionItem.Type == (int) ETransactionType.Charge)
                    {
                        throw new SecurityException($"Charging attempt without authorization. Token: {authToken}");
                    }

                    var transactionDetail = new TransactionDetail()
                    {
                        ItemNavigation = transactionItem,
                        Quantity = orderItem.Quantity,
                        Total = orderItem.Amount *
                                ((transactionItem.Type == (int) ETransactionType.Withdraw)
                                    ? -1
                                    : 1), // type 1 is withdraw
                        IsCancel = setCancelTransaction,
                    };

                    // adjust expiry date for transaction detail
                    var limitedCharges = existingTransactionDetails.Where(td =>
                        td.ItemNavigation.Type == (int) ETransactionType.Charge
                        && td.ValidTo != null
                    ).ToList();
                    // if transaction allowed and there exists limited charge then expire withdraw with that charge
                    if (limitedCharges.Any())
                    {
                        transactionDetail.ValidTo = limitedCharges.Max(td => td.ValidTo);
                    }

                    transactionDetails.Add(transactionDetail);
                }

                // check deposit rules
                if (card.Returnable == true)
                {
                    var transactionItemDeposit =
                        _db.TransactionItem.FirstOrDefault(ti => ti.SpecialCode == SPECIAL_CODE_DEPOSIT);
                    if (transactionItemDeposit == null)
                    {
                        var errorMessage = $"A returnable card is used but no deposit item is defined at system";
                        logger.Fatal(errorMessage);
                        return new RestResult<SaveTransactionResponse>(false, errorMessage, null);
                    }
                    // check existing transactions for deposit item
                    // if none exists add one to transaction
                    bool depositTaken = _db.TransactionDetail.Any(td =>
                        td.TransactionNavigation.CardNavigation == card
                        && td.ItemNavigation.SpecialCode == SPECIAL_CODE_DEPOSIT
                        && td.IsCancel != true
                    ); // checked if this item charged before

                    if (!depositTaken)
                    {
                        // deposit not taken so add deposit
                        transactionDetails.Add(new TransactionDetail()
                        {
                            ItemNavigation = transactionItemDeposit,
                            Quantity = 1,
                            Total = transactionItemDeposit.Price * -1,
                        });
                    }
                }

                //
                var totalAmount = transactionDetails.Sum(t => t.Total);

                var finalBalance = cardBalance + totalAmount;

                var minimumBalanceLimit = card.MinimumBalanceLimit.HasValue
                    ? card.MinimumBalanceLimit.Value
                    : site.MinimumBalanceLimit;

                if (finalBalance < minimumBalanceLimit)
                {
                    // todo log request for future inspections
                    return new RestResult<SaveTransactionResponse>(false,
                        $"New balance {finalBalance} is less than allowed limit {minimumBalanceLimit}", null);
                }

                var transaction = new Transaction
                {
                    DeviceLoginNavigation = deviceLogin,
                    EmployeeNavigation = deviceLogin?.EmployeeNavigation,
                    DeviceNavigation = deviceLogin?.DeviceNavigation,
                    CardNavigation = card,
                    Amount = totalAmount,
                    OldBalance = cardBalance,
                    NewBalance = finalBalance,
                    Cycle = 0,
                    Date = getSiteNow(),
                    SourceAddress = sourceAddress,
                    ProductType = iProductType,
                    TransactionDetail = transactionDetails,
                    TotalCharges = transactionDetails.Where(td => td.ItemNavigation.Type == 2).Sum(td => td.Total),
                    TotalSales = transactionDetails.Where(td => td.ItemNavigation.Type == 1).Sum(td => td.Total),
                    IsCancel = setCancelTransaction,
                };

                if (bypassChecks)
                {
                    if (site.CustomerProvisionTime.HasValue)
                    {
                        transaction.ProvisionDate = siteNow.AddMinutes(site.CustomerProvisionTime.Value);
                    }
                }
                else
                {
                    if (site.WaitressProvisionTime.HasValue)
                    {
                        transaction.ProvisionDate = siteNow.AddMinutes(site.WaitressProvisionTime.Value);
                    }
                }

                _db.Transaction.Add(transaction);

                errorCode = 110;

                if (!setCancelTransaction)
                {
                    // if any item defined to any station, set transaction item status to new order
                    CreateOrderFromTransaction(transaction, (EOrderType) request.OrderType, request.LocationCard);
                }

                errorCode = 120;
                _db.SaveChanges();

                if (site.NotifyCustomerForTransaction == true) // send customer notification for transaction
                {
                    var customerNavigationNotificationToken = card.CustomerNavigation.NotificationToken;
                    if (!string.IsNullOrWhiteSpace(customerNavigationNotificationToken))
                    {
                        List<string> messages = new List<string>();
                        if (transaction.TotalCharges.HasValue && transaction.TotalCharges != 0)
                        {
                            messages.Add($"${transaction.TotalCharges:0.##} charged on your card.");
                        }

                        if (transaction.TotalSales.HasValue && transaction.TotalSales != 0)
                        {
                            messages.Add($"${(-1m * transaction.TotalSales):0.##} deposited from your card.");
                        }

                        messages.Add($"Your final balance is ${transaction.NewBalance:0.##}.");
                        var message = string.Join(' ', messages);

                        sendPushMessage(customerNavigationNotificationToken, "Information", message);
                    }
                }

                return new RestResult<SaveTransactionResponse>(new SaveTransactionResponse()
                    {ConfirmationCode = null, TransactionId = transaction.Oid});
            }
            catch (Exception e)
            {
                logger.Error("Unable to save transaction", e);
                return new RestResult<SaveTransactionResponse>(false, $"Error {errorCode}",
                    new SaveTransactionResponse() {ConfirmationCode = ""});
            }
        }

        private void CreateOrderFromTransaction(Transaction transaction, EOrderType orderType, int? locationCard)
        {
            Order order = new Order();
            var customer = transaction.CardNavigation.CustomerNavigation;
            order.CustomerNavigation = customer;
            order.Date = transaction.Date;
            var orderDetails = new List<OrderDetail>();
            order.OrderDetail = orderDetails;
            order.TransactionNavigation = transaction;
            order.Type = (int?) orderType;

            var siteNow = getSiteNow();

            if (locationCard.HasValue) // if order belongs to a location, link it to location card
            {
                order.LocationCard = locationCard;
            }

            foreach (var td in transaction.TransactionDetail)
            {
                var stationItem = _db.StationItem.Include(si => si.StationNavigation)
                    .ThenInclude(s => s.EmployeeNavigation)
                    .FirstOrDefault(si => si.TransactionItemNavigation == td.ItemNavigation);
                if (stationItem != null)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.Quantity = td.Quantity;
                    orderDetail.TransactionItemNavigation = td.ItemNavigation;
                    orderDetail.TransactionDetailNavigation = td;
                    // find station if any
                    orderDetail.Station = stationItem.Station;
                    orderDetail.Status = (int?) EOrderStatus.New;

                    // todo Performance. Should be made async!!!

                    var pushDetailMessage = orderDetail.Quantity + " x " + orderDetail.TransactionItemNavigation.Name;
                    var pushTitleMessage = "New Order!";

                    // if item belongs to a station send notification to station employee
                    var notificationTokenStationEmployee =
                        stationItem.StationNavigation?.EmployeeNavigation?.NotificationToken;
                    if (!string.IsNullOrWhiteSpace(notificationTokenStationEmployee))
                    {
                        sendPushMessage(notificationTokenStationEmployee, pushTitleMessage,
                            pushDetailMessage);
                    }

                    // if order is from a location send notification to employee who opened location for card
                    if (locationCard.HasValue)
                    {
                        var notificationInformations = _db.LocationCard.Where(lc => lc.Oid == locationCard)
                            .Select(lc => new
                                {LocationName = lc.LocationNavigation.Name, lc.OpenedByNavigation.NotificationToken})
                            .First();
                        if (!string.IsNullOrWhiteSpace(notificationInformations.NotificationToken))
                        {
                            sendPushMessage(notificationInformations.NotificationToken,
                                $"Self Order! {customer.Name} ({notificationInformations.LocationName})",
                                pushDetailMessage);
                        }
                    }

                    orderDetails.Add(orderDetail);
                }
            }

            if (orderDetails.Any()) // if there are any order detail created for transaction then create order.
            {
                _db.Order.Add(order);
            }

            // warning! Saving should be done by caller method
        }

        private DeviceLogin DeviceLogin(string authToken, string deviceToken)
        {
            var deviceLogin = _db.DeviceLogin
                .Include(dl => dl.EmployeeNavigation).Include(dl => dl.DeviceNavigation)
                .First(
                    dl => dl.Token == authToken && dl.DeviceNavigation.DeviceToken == deviceToken);
            return deviceLogin;
        }

        private DateTime getSiteNow()
        {
            var site = getSite();
            var now = DateTime.UtcNow;
            var siteNow = now.AddHours((int) site.Timezone);
            return siteNow;
        }

        private Site getSite()
        {
            var site = _db.Site.First();
            return site;
        }

        [HttpPost]
        [Route("saveTransaction")]
        public RestResult<SaveTransactionResponse> SaveTransaction([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromBody] SaveTransactionRequest request)
        {
            var sourceAddress = Request.Host.Value; // todo check what is coming?
            int errorCode = 0;
            try
            {
                errorCode = 100;
                var deviceLogin = _db.DeviceLogin
                    .Include(dl => dl.EmployeeNavigation).Include(dl => dl.DeviceNavigation)
                    .First(
                        dl => dl.Token == request.TransactionToken && dl.DeviceNavigation.DeviceToken == deviceToken);

                errorCode = 101;
                // find and verify employee from Auth token, transaction token, imei and employee id
                // auth token can be different from transaction token if transaction was saved and new login done before sending tx.
                errorCode = 102;
                //

                errorCode = 103;
                var card = _db.Card.First(c => c.CardNo == request.CardNo);

                errorCode = 104;
                // create transaction
                var transaction = new Transaction
                {
                    DeviceLoginNavigation = deviceLogin,
                    EmployeeNavigation = deviceLogin.EmployeeNavigation,
                    DeviceNavigation = deviceLogin.DeviceNavigation,
                    CardNavigation = card,
                    Amount = request.Amount,
                    OldBalance = request.OldBalance,
                    NewBalance = request.NewBalance,
                    Cycle = request.Cycle,
                    Date = request.Date,
                    SourceAddress = sourceAddress
                };

                _db.Transaction.Add(transaction);

                errorCode = 110;
                _db.SaveChanges();
                return new RestResult<SaveTransactionResponse>(new SaveTransactionResponse() {ConfirmationCode = null});
            }
            catch (Exception e)
            {
                return new RestResult<SaveTransactionResponse>(false, $"Error {errorCode}",
                    new SaveTransactionResponse() {ConfirmationCode = ""});
            }
        }


        [HttpPost]
        [Route("report")]
        public RestResult<ReportResponse> Report([FromHeader] string authToken, [FromBody] ReportRequest reportRequest)
        {
            var deviceLogin = _db.DeviceLogin.Include(d => d.DeviceNavigation).First(dl => dl.Token == authToken);

            var itemsSoldByEmployee = _db.StationItem.Include(si => si.TransactionItemNavigation)
                .Where(si => si.StationNavigation.Employee == deviceLogin.Employee)
                .Select(si => si.TransactionItemNavigation.Oid);

            if (reportRequest.Start == null) reportRequest.Start = DateTime.Today;
            if (reportRequest.End == null) reportRequest.End = DateTime.Today.AddDays(1);

            var reportRequestStart = reportRequest.Start;
            var reportRequestEnd = reportRequest.End;

            var transactions =
                _db.Transaction.Where(td => td.Date >= reportRequestStart && td.Date <= reportRequestEnd);

            var chargeTransactions =
                transactions.Where(td => td.TransactionDetail.Any(tdt => tdt.ItemNavigation.Type == 2));
            var salesTransactions =
                transactions.Where(td => td.TransactionDetail.Any(tdt => tdt.ItemNavigation.Type == 1));

            IEnumerable<ReportOrder> orders = null;

            {
                if (itemsSoldByEmployee.Any()
                ) // employee is responsible for a station. Show orders only related with them
                {
                    var orderDetails = _db.OrderDetail
                        .Include(od => od.OrderNavigation).ThenInclude(o => o.CustomerNavigation)
                        .Include(od => od.OrderNavigation.LocationCardNavigation.LocationNavigation)
                        .Include(od => od.TransactionItemNavigation)
                        .Where(od => itemsSoldByEmployee.Contains(od.TransactionItemNavigation.Oid))
                        .Where(od =>
                            od.OrderNavigation.Date > reportRequestStart && od.OrderNavigation.Date < reportRequestEnd)
                        .OrderByDescending(od => od.OrderNavigation.Date).ToList();
                    // 
                    orders = orderDetails.Select(od => new ReportOrder()
                    {
                        SingleItem = true,
                        CustomerName = od.OrderNavigation.CustomerNavigation.Name,
                        Oid = od.OrderNavigation.Oid,
                        Date = od.OrderNavigation.Date,
                        Items = new List<ReportOrderItem>()
                        {
                            new ReportOrderItem()
                                {Quantity = od.Quantity, Name = od.TransactionItemNavigation.Name, Oid = od.Oid}
                        },
                        Status = od.Status,
                        Type = od.OrderNavigation.Type,
                        Location = od.OrderNavigation.LocationCardNavigation?.LocationNavigation?.Name,
                    }).ToList();
                }
                else // employee is not at station. Show all orders
                {
                    var orderDetails = _db.OrderDetail
                        .Include(od => od.OrderNavigation).ThenInclude(o => o.CustomerNavigation)
                        .Include(od => od.TransactionItemNavigation)
                        .Include(od => od.OrderNavigation.LocationCardNavigation.LocationNavigation)
                        .Where(od =>
                            od.OrderNavigation.Date > reportRequestStart && od.OrderNavigation.Date < reportRequestEnd)
                        .Where(od =>
                            od.OrderNavigation.Type ==
                            (int?) EOrderType
                                .None) // only show ordinary orders. Not delivery or pickup. // todo make this parametric
                        .Where(od =>
                            od.TransactionDetailNavigation.TransactionNavigation.Employee == deviceLogin.Employee)
                        .OrderByDescending(od => od.OrderNavigation.Date).ToList();
                    // 
                    orders = orderDetails.Select(od => new ReportOrder()
                    {
                        SingleItem = true,
                        CustomerName = od.OrderNavigation.CustomerNavigation.Name,
                        Oid = od.OrderNavigation.Oid,
                        Date = od.OrderNavigation.Date,
                        Items = new List<ReportOrderItem>()
                        {
                            new ReportOrderItem()
                                {Quantity = od.Quantity, Name = od.TransactionItemNavigation.Name, Oid = od.Oid}
                        },
                        Status = od.Status,
                        Type = od.OrderNavigation.Type,
                        Location = od.OrderNavigation.LocationCardNavigation?.LocationNavigation?.Name,
                    });
                }
            }

            if (reportRequest.Orders)
            {
                // orders requested. Convert to list. Iterate
                orders = orders.ToList();
                _FilterAndSortOrdersAccordingToDevice((List<ReportOrder>) orders, deviceLogin.DeviceNavigation);
            }
            // todo authToken kullanımı eklenecek

            //            var deviceLogin = _db.DeviceLogin.First(dl => dl.Token == authToken);
            var reportResponse = new ReportResponse()
            {
                CountCharges = reportRequest.Charges ? chargeTransactions.Count() : (int?) null,
                CountSales = reportRequest.Sales ? salesTransactions.Count() : (int?) null,
                TotalCharges = reportRequest.Charges
                    ? (chargeTransactions.Sum(td => td.TotalCharges) ?? 0)
                    : (decimal?) null,
                TotalSales = reportRequest.Sales ? (salesTransactions.Sum(td => td.TotalSales) ?? 0) : (decimal?) null,
                CountCustomer = _db.Customer.Count(),
                Charges = reportRequest.Charges
                    ? chargeTransactions.Select(
                        t => new ReportTransaction()
                        {
                            Amount = t.TotalCharges ?? 0,
                            Balance = t.NewBalance ?? 0,
                            CardNo = t.CardNavigation.CardNo,
                            Date = t.Date,
                            Name = t.CardNavigation.CustomerNavigation.Name
                        }).OrderByDescending(t => t.Date)
                    : null,
                Sales = reportRequest.Sales
                    ? salesTransactions.Select(
                        t => new ReportTransaction()
                        {
                            Amount = t.TotalSales ?? 0,
                            Balance = t.NewBalance ?? 0,
                            CardNo = t.CardNavigation.CardNo,
                            Date = t.Date,
                            Name = t.CardNavigation.CustomerNavigation.Name
                        }).OrderByDescending(t => t.Date)
                    : null,
                Orders = reportRequest.Orders
                    ? orders
                    : null
            };

            return new RestResult<ReportResponse>(true, null, reportResponse);
        }

        private List<ReportOrder> _FilterAndSortOrdersAccordingToDevice(List<ReportOrder> orders, Device device)
        {
            var now = getSiteNow();

            var completedOrderExpireTime = now.AddMinutes((device.CompletedOrderKeepDuration ?? 60) * -1);

            if (device.KioskMode == true)
            {
                // show only completed orders
                orders.RemoveAll(o => o.Status != (int) EOrderStatus.Completed);
            }

            // dispose old completed orders
            orders.RemoveAll(o =>
                (o.Status == (int) EOrderStatus.Canceled || o.Status == (int) EOrderStatus.Completed) &&
                o.Date < completedOrderExpireTime);
            ReportOrder r = new ReportOrder();

            if (device.MaskOrderCustomer == true)
            {
                orders.ForEach(o => o.CustomerName = o.CustomerName.Mask());
            }

            foreach (var reportOrder in orders.Where(o => !o.Status.HasValue))
            {
                reportOrder.Status = 0;
            }

            return orders.OrderBy(o => o.Status).ThenByDescending(o => o.Date).ToList();
        }

        [HttpGet]
        [Route("ClientAvailable")]
        public RestResult<bool> ClientNameAvailable(string clientName)
        {
            var tenantDbName = "epay3." + clientName;
            var command = _db.Database.GetDbConnection().CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "select name from sys.databases";
            _db.Database.OpenConnection();
            using (var result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    if (tenantDbName == (string) result[0])
                    {
                        // tenant exists
                        return new RestResult<bool>(true, null, false);
                    }
                }
            }

            _db.Database.CloseConnection();

            return new RestResult<bool>(true, null, false);
        }

        [HttpPost]
        [Route("CompleteOrder")]
        public RestResult<CompleteOrderResponse> CompleteOrder([FromHeader] string customerToken,
            [FromBody] CompleteOrderRequest completeOrderRequest)
        {
            if (string.IsNullOrWhiteSpace(customerToken)) throw new SecurityException("Customer token must exist");
            var customer = findCustomer(customerToken);

            var site = getSite();

            var siteNow = getSiteNow();
            logger.Debug($"Creating an order for {customer.Name} at {siteNow.ToLongTimeString()}");
            // 60 seconds between orders check
            var waitBetweenOrders = (int) site.OrderCooldownTime;
            var lastOrder = _db.Order.FirstOrDefault(o =>
                o.CustomerNavigation == customer && o.Date > siteNow.AddSeconds(-waitBetweenOrders));
            if (lastOrder != null)
            {
                int waitForTime = (int) (waitBetweenOrders - (siteNow - lastOrder.Date).Value.TotalSeconds);
                return new RestResult<CompleteOrderResponse>(false,
                    "Please wait " + waitForTime + " seconds for a new order", null);
            }


            var order = new Order();

            order.Date = siteNow;

            foreach (var orderItem in completeOrderRequest.OrderItems.Where(i => i.Amount > 0))
            {
                var orderDetail = new OrderDetail();
                orderDetail.TransactionItem = orderItem.IdTransactionItem;
                orderDetail.Quantity = orderItem.Amount;
                order.OrderDetail.Add(orderDetail);
            }

            customer.Order.Add(order);
            _db.SaveChanges();

            var orderDetails = _db.OrderDetail.Where(od => od.Order == order.Oid).Select(od =>
                new {od.Quantity, od.TransactionItemNavigation.Name, od.TransactionItemNavigation.Price}).ToList();

            var orderDetailAsString = string.Join(", ", orderDetails.Select(it => $"{it.Quantity} {it.Name}"));

            sendPushMessage(_appTenant.Client, order.CustomerNavigation.Name, orderDetailAsString);

            return new RestResult<CompleteOrderResponse>(true, null, null);
        }

        [HttpPost]
        [Route("customer/register")]
        public RestResult<CustomerRegisterResponse> CustomerRegister([FromBody] CustomerRegisterRequest registerRequest)
        {
            var phoneNumber = registerRequest.PhoneNumber.NormalizePhoneNumber("+90", 10);

            var customer = _db.Customer.FirstOrDefault(c => c.Phone == phoneNumber);

            if (customer == null)
            {
                var message = "Phone number is not found!";

                return new RestResult<CustomerRegisterResponse>(false, message, null);
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                var message = "Number invalid! Please check your phone number";
                logger.Warn(new { message = message, data = phoneNumber });
                return new RestResult<CustomerRegisterResponse>(false,
                    message, null);
            }

            var registration = _db.RegistrationRequest.Include(r => r.CustomerNavigation)
                .FirstOrDefault(r => r.CustomerNavigation.Phone == phoneNumber && r.DateConsumed == null);

            if (registration != null)
            {
                // Sms sent previously. Check cooldown time 
#if !DEVELOPMENT
                var retryCooldownMin = 1;
#else
                var retryCooldownMin = 0;
#endif
                if (DateTime.Now.AddMinutes(-retryCooldownMin) < registration.DateSent)
                {
                    return new RestResult<CustomerRegisterResponse>(true,
                        $"In {retryCooldownMin} minutes only one sms message allowed. Please wait..",
                        new CustomerRegisterResponse()
                            {IsSent = true, LastSentDate = registration.DateSent});
                }
            }

            // save sms send request

            if (registration == null)
            {
#if DEVELOPMENT
                var randomVerificationCode = "1234";
#else
                var randomVerificationCode = CommonExtensions.GenerateRandomNumberCode(4);
#endif

                logger.Warn(new { message = "Verification code: "+randomVerificationCode, data = phoneNumber });

                registration = new RegistrationRequest()
                    {CustomerNavigation = customer, VerificationCode = randomVerificationCode, DateSent = getSiteNow()};
            }

            _db.RegistrationRequest.Update(registration);
            _db.SaveChanges();

#if !DEVELOPMENT
            // send sms message
            _messagingService.sendSms(phoneNumber,
                $"Using {registration.VerificationCode} code you can complete your registration.", _appTenant.Client);
#endif

            return new RestResult<CustomerRegisterResponse>(new CustomerRegisterResponse()
                {IsSent = true, LastSentDate = registration.DateSent});
        }

        [HttpPost]
        [Route("customer/verify")]
        public RestResult<CustomerVerifyResponse> CustomerVerify([FromBody] CustomerVerifyRequest verifyRequest)
        {
            // normalize phone number
            var normalizePhoneNumber = verifyRequest.PhoneNumber.NormalizePhoneNumber("+90", 10);

            // try verify request
            var registrationRequest = _db.RegistrationRequest.Include(r => r.CustomerNavigation).FirstOrDefault(r =>
                r.CustomerNavigation.Phone == normalizePhoneNumber && r.DateConsumed == null &&
                r.VerificationCode == verifyRequest.VerificationCode);

            if (registrationRequest == null)
            {
                var message = "Registrationn failed. Please retry again.";
                logger.Warn(new {data=normalizePhoneNumber,message = message,detail=$"Registration request not found! Verification code received is {verifyRequest.VerificationCode}"});
                return new RestResult<CustomerVerifyResponse>(false, message, null);
            }

            // Verification successful!

            registrationRequest.DateConsumed = DateTime.Now;

            //if (string.IsNullOrWhiteSpace(registrationRequest.CustomerNavigation.Token))
            registrationRequest.CustomerNavigation.Token =
                GenerateCustomerNavigationToken(registrationRequest.CustomerNavigation);

            _db.SaveChanges();

            return new RestResult<CustomerVerifyResponse>(new CustomerVerifyResponse()
                {Token = registrationRequest.CustomerNavigation.Token});
        }

        private string GenerateCustomerNavigationToken(Customer customer)
        {
            var phone = customer.Phone;
            CustomerToken token = new CustomerToken()
            {
                Client = _appTenant.Client + "*",
                Phone = customer.Phone,
                NotificationToken = customer.NotificationToken
            };
            var tokenPlain = JsonConvert.SerializeObject(token);
            var tokenEncrypted = "v01:" + tokenPlain.Encrypt("FILL_RANDOM_SALT");
            return tokenEncrypted;
        }


        [HttpPost]
        [Route("customer/makeorder")]
        public RestResult<CustomerReportResponse> CustomerMakeOrder([FromHeader] string customerToken,
            [FromBody] CustomerOrderRequest customerOrderRequest)
        {
            //
            // make new order
            //
            var siteNow = getSiteNow();
            var site = getSite();
            // find customer
            var customer = findCustomer(customerToken);

            // check if has any open location
            // var locationCard = _db.LocationCard.Include(lc => lc.CardNavigation).FirstOrDefault(cl =>
            //     cl.OpenedAt < siteNow && (cl.ClosedAt == null || cl.ClosedAt > siteNow) &&
            //     cl.CardNavigation.CustomerNavigation == customer);
            //var lcxs = _db.LocationCard.FirstOrDefault(lcx => lcx.Oid == customerOrderRequest.IdCardLocation);

            var locationCard = _db.LocationCard
                .Include(lc=>lc.CardNavigation)
                .Include(lc=>lc.CardNavigation)
                .Include(lc => lc.DeviceLoginNavigation.EmployeeNavigation)
                .Include(lc => lc.DeviceLoginNavigation.DeviceNavigation)
                .FirstOrDefault(lc => lc.Oid == customerOrderRequest.IdCardLocation
                                                                     && (lc.OpenedAt < siteNow && (lc.ClosedAt == null ||
                                                                         lc.ClosedAt > siteNow)));

            // if no location sent, check if remote orders enabled.
            if (locationCard == null && !site.EnableRemoteOrders == true)
            {
                // Not enabled! Return error
                return new RestResult<CustomerReportResponse>(false, "Remote orders are not allowed.", null);
            }

            if (locationCard != null && !site.EnableLocationOrders == true)
            {
                return new RestResult<CustomerReportResponse>(false, "Order from location is not allowed.", null);
            }

            bool locationExists = (locationCard != null);

            // if (site.EnableLocationOrders == true && !locationExists) // RemoteAttribute order enabled but customer has no active location
            // {
            //     return new RestResult<CustomerReportResponse>(false,
            //         "Açık masa bulunamadı. Lütfen adınıza masa açıldığından emin olunuz.", null);
            // }

            // make a new transaction
            var saveOnlineTransactionRequest = new SaveOnlineTransactionRequest();

            if (locationExists)
            {
                // use card of location
                saveOnlineTransactionRequest.CardNo = locationCard.CardNavigation.CardNo;
                saveOnlineTransactionRequest.LocationCard = locationCard.Oid;
            }
            else
            {
                // use first active card
                var firstActiveCard = _db.Card.First(c => c.Active == true && c.Customer == customer.Oid);
                saveOnlineTransactionRequest.CardNo = firstActiveCard.CardNo;
                saveOnlineTransactionRequest.OrderType = (int) EOrderType.External;
            }

            saveOnlineTransactionRequest.OrderItems = customerOrderRequest.orderItems;
            try
            {
                string authToken = null;
                string deviceToken = null;
                if (locationExists && locationCard.DeviceLoginNavigation != null
                ) // is customer sat at a location? If so link order to waitress who made guest sat
                {
                    authToken = locationCard.DeviceLoginNavigation.Token;
                    deviceToken = locationCard.DeviceLoginNavigation.DeviceNavigation.DeviceToken;
                    bypassChecks = false;
                }
                else
                {
                    bypassChecks = true;
                }

                var saveOnlineTransaction = SaveOnlineTransaction(authToken, deviceToken,
                    ((int) EProductType.Money).ToString(),
                    saveOnlineTransactionRequest);

                var customerReport = CustomerReport(customerToken);

                customerReport.Success = saveOnlineTransaction.Success;
                customerReport.Message = saveOnlineTransaction.Message;

                return customerReport;
            }
            finally
            {
                bypassChecks = false;
            }
        }

        [HttpGet]
        [Route("customer/report")]
        public RestResult<CustomerReportResponse> CustomerReport([FromHeader] string customerToken)
        {
            //
            // make new order
            //
            var siteNow = getSiteNow();
            // find customer
            var customer = findCustomer(customerToken);

            CustomerReportResponse reportResponse = new CustomerReportResponse();

            reportResponse.Orders = _db.OrderDetail.Where(od =>
                    od.OrderNavigation.Date > siteNow.Date && od.OrderNavigation.Customer == customer.Oid)
                .Select(od => new CustomerReportOrder
                {
                    Status = od.Status, Name = od.TransactionItemNavigation.Name, Quantity = od.Quantity,
                    Date = od.OrderNavigation.Date, IdItem = od.TransactionItemNavigation.Oid,
                    Price = od.TransactionItemNavigation.Price,
                    Amount = od.Quantity * od.TransactionItemNavigation.Price,
                    Image = od.TransactionItemNavigation.Image
                }).ToList();

            reportResponse.Transactions = _db.TransactionDetail
                .Where(t => t.TransactionNavigation.CardNavigation.CustomerNavigation == customer)
                .OrderByDescending(td => td.TransactionNavigation.Date)
                .Select(td => new CustomerReportTransactionDetail()
                {
                    Date = td.TransactionNavigation.Date,
                    Quantity = td.Quantity,
                    Total = td.Total,
                    Name = td.ItemNavigation.Name
                }).ToList();


            var card = _db.Card.FirstOrDefault(c => c.Active == true && c.CustomerNavigation == customer);
            if (card == null)
            {
                return new RestResult<CustomerReportResponse>(false, "No card found for the customer",
                    new CustomerReportResponse());
            }

            bypassChecks = true;
            var restResultCardResponse = GetCard(null, new GetCardRequest() {CardNo = card.CardNo},
                ((int) EProductType.Money).ToString());
            reportResponse.Card = restResultCardResponse.Data;

            var locationCardResponses = _db.LocationCard
                .Where(lc => (lc.OpenedAt < siteNow && (lc.ClosedAt == null || lc.ClosedAt > siteNow)))
                .Where(lc => lc.CardNavigation == card)
                .Select(lc => new LocationCardResponse()
                {
                    CardNo = card.CardNo,
                    Name = lc.LocationNavigation.Name,
                    OpenedAt = lc.OpenedAt,
                    Id = lc.Oid
                })
                .ToList();

            reportResponse.LocationCards = locationCardResponses;


            reportResponse.CustomerName = customer.Name;
            reportResponse.CustomerKey = customer.Phone;

            if (string.IsNullOrWhiteSpace(customer.NotificationToken))
            {
                // create and save notification token
                customer.NotificationToken = Guid.NewGuid().ToString();
                _db.SaveChanges();
            }

            reportResponse.CustomerNotificationToken = customer.NotificationToken;
            
            return new RestResult<CustomerReportResponse>(true, null, reportResponse);
        }

        private Customer findCustomer(string customerToken)
        {
            Customer customer;

            var indexOfSeperator = customerToken.IndexOf(':');
            if (indexOfSeperator > 0)
            {
                var version = customerToken.Substring(0, indexOfSeperator);
                var encryptedToken = customerToken.Substring(indexOfSeperator + 1);

                if (version == "v01")
                {
                    var plainToken = encryptedToken.Decrypt("FILL_RANDOM_SALT");
                    var token = JsonConvert.DeserializeObject<CustomerToken>(plainToken);
                    var tokenClient = token.Client;
                    var phoneNumber = token.Phone;
                    bool clientAuthorized = false;
                    if (tokenClient.EndsWith("*"))
                    {
                        // do wildcard check
                        var tokenSubToken = tokenClient.Substring(0, tokenClient.Length - 1);
                        clientAuthorized =
                            _appTenant.Client
                                .StartsWith(
                                    tokenSubToken);
                    }
                    else
                    {
                        clientAuthorized = _appTenant.Client == tokenClient;
                    }

                    if (!clientAuthorized)
                    {
                        throw new SecurityException("Error 8888");
                    }

                    customer = _db.Customer.First(c => c.Phone == phoneNumber);
                    customer.NotificationToken = token.NotificationToken;
                }
                else
                {
                    throw new SecurityException("Invalid token");
                }
            }
            else
            {
                customer = _db.Customer.First(c => c.Token == customerToken);
            }

            return customer;
        }

        [HttpGet]
        [Route("waitress/locations")]
        public RestResult<List<LocationResponse>> WaitressLocations([FromHeader] string authToken,
            [FromHeader] string deviceToken)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();
            var locationResponse = _db.Location.Include(l => l.LocationCard)
                .ThenInclude(lc => lc.CardNavigation)
                .ThenInclude(c => c.CustomerNavigation).Select(loc => new
                    LocationResponse()
                    {
                        Id = loc.Oid,
                        Name = loc.Name,
                        LocationCards = loc.LocationCard.Where(l => l.ClosedAt == null || l.ClosedAt > siteNow).Select(
                            lc => new LocationCardResponse()
                            {
                                CardNo = lc.CardNavigation.CardNo,
                                Name = lc.CardNavigation.CustomerNavigation.Name,
                                OpenedAt = lc.OpenedAt
                            })
                    }).ToList();

            var locationResponses = new List<LocationResponse>();
            return new RestResult<List<LocationResponse>>(true, null, locationResponse);
        }

        [HttpGet]
        [Route("waitress/location/{id}")]
        public RestResult<LocationResponse> WairtessLocation([FromHeader] string authToken,
            [FromHeader] string deviceToken, int id)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();

            var locationResponse = _db.Location.Include(l => l.LocationCard)
                .ThenInclude(lc => lc.CardNavigation)
                .ThenInclude(c => c.CustomerNavigation)
                .Where(loc => loc.Oid == id)
                .Select(loc => new
                    LocationResponse()
                    {
                        Id = loc.Oid,
                        Name = loc.Name,
                        LocationCards = loc.LocationCard.Where(l => l.ClosedAt == null || l.ClosedAt > siteNow).Select(
                            lc => new LocationCardResponse()
                            {
                                CardNo = lc.CardNavigation.CardNo,
                                Name = lc.CardNavigation.CustomerNavigation.Name,
                                OpenedAt = lc.OpenedAt
                            })
                    }).First();

            return new RestResult<LocationResponse>(true, null, locationResponse);
        }

        [HttpGet]
        [Route("customer/image/item/{id}")]
        public byte[] CustomerItemImage(int id)
        {
            var customerItemImage =
                _db.TransactionItem.Where(ti => ti.Oid == id).Select(ti => ti.Image).FirstOrDefault();

            if (customerItemImage == null)
            {
                // return placeholder image
            }

            return customerItemImage;
        }

        // open location
        [HttpPost]
        [Route("waitress/addcardtolocation")]
        public RestResult<LocationResponse> WaitressLocationAddCard([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromBody] LocationAddCardRequest addCardRequest)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();
            // does exists?
            var location = _db.Location.First(l => l.Oid == addCardRequest.LocationId);
            var card = _db.Card.FirstOrDefault(c => c.CardNo == addCardRequest.CardNo && c.Active == true);
            if (card == null)
            {
                return new RestResult<LocationResponse>(false, "Card not found!", null);
            }

            var locationCard = _db.LocationCard.FirstOrDefault(lc => lc.LocationNavigation == location
                                                                     && lc.CardNavigation == card
                                                                     && lc.OpenedAt < siteNow
                                                                     && (lc.ClosedAt == null || lc.ClosedAt > siteNow)
            );

            if (locationCard != null)
            {
                return WaitressLocationRemoveCard(authToken, deviceToken,
                    new LocationRemoveCardRequest()
                        {CardNo = addCardRequest.CardNo, LocationId = addCardRequest.LocationId});
                // card exists
//                return new RestResult<LocationResponse>(false, "Card already exists at location", null);
            }

            // card does not exists. Create a new one
            var newLocationCard = new LocationCard()
            {
                LocationNavigation = location, CardNavigation = card, OpenedAt = siteNow.AddMinutes(-1),
                DeviceLoginNavigation = deviceLogin,
                OpenedByNavigation = deviceLogin.EmployeeNavigation
            };
            _db.LocationCard.Add(newLocationCard);
            _db.SaveChanges();

            return WairtessLocation(authToken, deviceToken, addCardRequest.LocationId);
        }

        // close location
        [HttpPost]
        [Route("waitress/removecardfromlocation")]
        public RestResult<LocationResponse> WaitressLocationRemoveCard([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromBody] LocationRemoveCardRequest removeCardRequest)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();
            // does exists?
            var location = _db.Location.First(l => l.Oid == removeCardRequest.LocationId);
            var card = _db.Card.First(c => c.CardNo == removeCardRequest.CardNo && c.Active == true);

            var locationCard = _db.LocationCard.FirstOrDefault(lc => lc.LocationNavigation == location
                                                                     && lc.CardNavigation == card
                                                                     && lc.OpenedAt < siteNow
                                                                     && (lc.ClosedAt == null || lc.ClosedAt > siteNow)
            );

            if (locationCard == null)
            {
                // card exists
                return new RestResult<LocationResponse>(false, "Card does not exists at location", null);
            }

            // card does not exists. Create a new one
            locationCard.ClosedAt = siteNow;

            _db.SaveChanges();

            return WairtessLocation(authToken, deviceToken, removeCardRequest.LocationId);
        }

        [HttpPost]
        [Route("waitress/station/updateorderstatus")]
        public RestResult<ReportResponse> WaitressUpdateOrderStatus([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromBody] UpdateOrderStatusRequest updateOrderStatusRequest)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();

            logger.Debug(
                $"Updating order detail({updateOrderStatusRequest.IdOrderDetail}) status to {updateOrderStatusRequest.Status}");

            if (updateOrderStatusRequest.IdOrder.HasValue)
            {
                // todo change status of all order items and order
                return new RestResult<ReportResponse>(false, "Update by order id not implemented", null);
            }
            else
            {
                var orderDetail = _db.OrderDetail
                    .Include(od => od.TransactionDetailNavigation.TransactionNavigation.EmployeeNavigation)
                    .Include(od => od.TransactionItemNavigation)
                    .Include(od => od.OrderNavigation.CustomerNavigation)
                    .Include(od => od.TransactionDetailNavigation.TransactionNavigation.CardNavigation)
                    .First(o => o.Oid == updateOrderStatusRequest.IdOrderDetail);

                EOrderStatus oldStatus = (EOrderStatus) orderDetail.Status; // todo improve code Status can be null
                EOrderStatus newStatus = (EOrderStatus) updateOrderStatusRequest.Status;

                bool changeSuccess = false;
                string changeMessage = null;

                if (oldStatus == EOrderStatus.Canceled)
                {
                    logger.Debug("Order was canceled. No operation can be done");
                    changeSuccess = false;
                    changeMessage = "No operation allowed on a canceled transaction!";
                }
                else if (oldStatus == EOrderStatus.Completed)
                {
                    logger.Debug("Order was completed. No operation can be done");
                    changeSuccess = false;
                    changeMessage = "No operation allowed on a completed transaction!";
                }
                else if (oldStatus != newStatus)
                {
                    logger.Debug($"Order status will be updated from {oldStatus} to {newStatus}");

                    if (newStatus == EOrderStatus.Canceled && orderDetail.TransactionDetailNavigation != null
                    ) // order is cancelling and payment already taken. Create counter record
                    {
                        var transaction = orderDetail.TransactionDetailNavigation
                            .TransactionNavigation;
                        var card = transaction.CardNavigation;
                        var isCancelRequestBeforeProvisionDate =
                            transaction.ProvisionDate.HasValue && (siteNow < transaction.ProvisionDate);
                        if (isCancelRequestBeforeProvisionDate)
                        {
                            // create counter record
                            setCancelTransaction = true;
                            var saveOnlineTransactionRequest = new SaveOnlineTransactionRequest();
                            saveOnlineTransactionRequest.CardNo = card.CardNo;
                            var cancelQuantity =
                                (decimal) (-1 * orderDetail
                                    .Quantity); // quantity multiplied by -1 to create inverse record
                            saveOnlineTransactionRequest.OrderItems = new List<OrderItem>();
                            saveOnlineTransactionRequest.OrderItems.Add(new OrderItem()
                            {
                                Quantity = cancelQuantity,
                                IdTransactionItem = orderDetail.TransactionItemNavigation.Oid,
                                Amount = ((decimal) orderDetail.TransactionItemNavigation.Price) *
                                         cancelQuantity
                            });
                            SaveOnlineTransaction(authToken, deviceToken,
                                orderDetail.TransactionItemNavigation.ProductType.ToString(),
                                saveOnlineTransactionRequest);

                            // set status for order
                            orderDetail.Status = (int) newStatus;
                        }
                        else
                        {
                            changeMessage = "Sipariş provizyon süresi dolduğu için iptal edilemez!";
                            changeSuccess = false;
                        }
                    }
                    else
                    {
                        orderDetail.Status = (int) newStatus;
                    }

                    _db.SaveChanges();

                    changeSuccess = true;

                    // trigger onesignal push notification
                    if (orderDetail.TransactionDetailNavigation != null) // we assume order has transaction link
                    {
                        var message =
                            $"{orderDetail.OrderNavigation.CustomerNavigation.Name} - {orderDetail.TransactionItemNavigation.Name}";

                        // employee 1 is the waitress who create transaction
                        var notificationTokenEmployee1 = orderDetail.TransactionDetailNavigation.TransactionNavigation
                            ?.EmployeeNavigation?.NotificationToken;

                        // employee 2 is the waitress who opened location
                        // find location opened for the card and send notification to that employer
                        //var card = orderDetail?.TransactionDetailNavigation?.TransactionNavigation?.CardNavigation;
                        var locationCard = _db.LocationCard.FirstOrDefault(lc =>
                            (lc.ClosedAt == null || lc.ClosedAt > siteNow) && lc.Card ==
                            orderDetail.TransactionDetailNavigation.TransactionNavigation.Card);
                        var notificationTokenEmployee2 = locationCard?.OpenedByNavigation?.NotificationToken;

                        // cutomer token
                        var notificationTokenCustomer = orderDetail.TransactionDetailNavigation.TransactionNavigation
                            ?.CardNavigation?.CustomerNavigation?.NotificationToken;


                        string orderNotificationHeader = null;
                        switch (newStatus)
                        {
                            case EOrderStatus.Completed:
                                orderNotificationHeader = "Sipariş Hazır";
                                break;
                            case EOrderStatus.InProgress:
                                orderNotificationHeader = "Sipariş Hazırlanıyor";
                                break;
                            case EOrderStatus.Canceled:
                                orderNotificationHeader = "Sipariş iptal edildi";
                                break;
                        }

                        logger.Debug(
                            $"Push notification token employee 1 (created transaction): {notificationTokenEmployee1}");
                        logger.Debug(
                            $"Push notification token employee 2 (opened location transaction): {notificationTokenEmployee2}");
                        logger.Debug($"Push notification token customer (customer): {notificationTokenCustomer}");
                        if (!string.IsNullOrWhiteSpace(orderNotificationHeader))
                        {
                            if (notificationTokenEmployee1 != null)
                            {
                                // todo customize message to inform more (include location and customer info)
                                sendPushMessage(notificationTokenEmployee1, orderNotificationHeader, message);
                            }

                            if (notificationTokenEmployee2 != null)
                            {
                                // todo customize message to inform more (include location and customer info)
                                sendPushMessage(notificationTokenEmployee2, orderNotificationHeader, message);
                            }

                            if (notificationTokenCustomer != null)
                            {
                                sendPushMessage(notificationTokenCustomer, orderNotificationHeader, message);
                            }
                        }
                    }
                }

                var reportResponse = Report(authToken, new ReportRequest() {Orders = true});
                reportResponse.Success = changeSuccess;
                reportResponse.Message = changeMessage;
                return reportResponse;
            }
        }

        private void sendPushMessage(string notificationToken, string title, string message)
        {
            logger.Debug(new
            {
                message = "Sending push notification.", title = title, content = message, token = notificationToken
            });
            var customer = _db.Customer.FirstOrDefault(c => c.NotificationToken == notificationToken);
            if (customer != null)
            {
                var oneSignalResponse = _oneSignalService.SendToClient(
                    notificationToken
                    , title,
                    message);
                logger.Debug(new {message = "One signal response", data = oneSignalResponse});
                Message msg = new Message();
                msg.ToNavigation = customer;
                msg.Date = getSiteNow();
                msg.Title = title;
                msg.Content = message;
                _db.Message.Add(msg);
                _db.SaveChanges();
            }
            else
            {
                logger.Warn("Unable to send message '" + message + "' to customer with token " + notificationToken +
                            ". Customer not found!");
            }
        }

        [HttpPost]
        [Route("waitress/order/cancel")]
        public RestResult<CancelOrderResponse> WaitressCancelOrder([FromHeader] string authToken,
            [FromHeader] string deviceToken, [FromBody] CancelOrderRequest cancelOrderRequest)
        {
            var deviceLogin = DeviceLogin(authToken, deviceToken);
            var siteNow = getSiteNow();

            // order bağlı transaction bul
            var order = _db.Order
                .Include(o => o.TransactionNavigation)
                .Include(o => o.OrderDetail).ThenInclude(od => od.TransactionDetailNavigation)
                .First(o => o.Oid == cancelOrderRequest.IdOrder);
            var transaction = order.TransactionNavigation;
            if (transaction == null)
            {
                return new RestResult<CancelOrderResponse>(false, "No transaction related with order!", null);
            }

            // provizyon saati kontrol et. Uygun değilse hata dön
            var isCancelRequestBeforeProvisionDate =
                transaction.ProvisionDate.HasValue && (siteNow < transaction.ProvisionDate);
            if (!isCancelRequestBeforeProvisionDate)
            {
                return new RestResult<CancelOrderResponse>(false, "Order can not be canceled because provision time expired!",
                    null);
            }

            // -- order detail'e ait to transaction detaillerin transactionları aynı mı kontrol et

            bool allBelongsToSameTransaction =
                order.OrderDetail.All(od => od.TransactionDetailNavigation.Transaction == transaction.Oid);
            if (!allBelongsToSameTransaction)
            {
                return new RestResult<CancelOrderResponse>(false,
                    "Order can not be canceled since it belongs to more than one transaction!", null);
            }

            // order ile olan bağlantıları kopar ve order statülerini iptal yap
            foreach (var orderDetail in order.OrderDetail)
            {
                orderDetail.TransactionDetailNavigation = null;
                orderDetail.Status = (int) EOrderStatus.Canceled;
            }

            order.TransactionNavigation = null;
            order.Status = (int) EOrderStatus.Canceled;

            // transaction sil
            _db.RemoveRange(transaction.TransactionDetail);
            _db.Remove(transaction);

            // todo Important! Silme işlemini logla
            _db.SaveChanges();

            return new RestResult<CancelOrderResponse>(true, new CancelOrderResponse());
        }

        [HttpPost]
        [Route("customer/messages")]
        public RestResult<List<MessageResponse>> CustomerMessages([FromHeader] string customerToken)
        {
            var siteNow = getSiteNow();
            // find customer
            var customer = findCustomer(customerToken);
            var messageResponses = _db.Message.Where(m => m.Date > siteNow.AddDays(-7) && m.ToNavigation == customer)
                .OrderByDescending(m => m.Date)
                .Select(m => new MessageResponse() {Date = m.Date, Title = m.Title, Content = m.Content}).ToList();

            return new RestResult<List<MessageResponse>>(messageResponses);
        }

        [HttpGet]
        [Route("clients")]
        public RestResult<ClientInfoResponse> Clients(string groupName)
        {
            ClientInfoResponse response = new ClientInfoResponse();

            if (groupName == "epay")
            {
                response.ClientGroups.Add(new ClientGroup("Epay", new List<ClientInfo>(new[]
                {
                    new ClientInfo("Epay", "epay"),
                })));

            }
            else
            {
                return new RestResult<ClientInfoResponse>(false, "Client group doesn't exist", null);
            }

            return new RestResult<ClientInfoResponse>(response);
        }
    }

    public class CustomerOrderRequest
    {
        public List<OrderItem> orderItems { get; set; }
        public Int32? IdCardLocation { get; set; }
    }

    public class MessageResponse
    {
        public DateTime? Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class ClientInfoResponse
    {
        public List<ClientGroup> ClientGroups { get; set; }

        public ClientInfoResponse()
        {
            ClientGroups = new List<ClientGroup>();
        }
    }

    public class ClientGroup
    {
        public string Name { get; set; }

        public List<ClientInfo> Clients { get; set; }

        public ClientGroup()
        {
            Clients = new List<ClientInfo>();
        }

        public ClientGroup(string name, List<ClientInfo> clients)
        {
            Name = name;
            Clients = clients;
        }
    }

    public class ClientInfo
    {
        public string Name { get; set; }
        public string ClientCode { get; set; }

        public ClientInfo()
        {
        }

        public ClientInfo(string name, string clientCode)
        {
            Name = name;
            ClientCode = clientCode;
        }
    }


    public class CustomerReportResponse
    {
        public IEnumerable<CustomerReportOrder> Orders { get; set; }
        public IList<CustomerReportTransactionDetail> Transactions { get; set; }
        public IList<LocationCardResponse> LocationCards { get; set; }
        public CardResponse Card { get; set; }
        public string CustomerName { get; set; }
        public string CustomerKey { get; set; }
        public string CustomerNotificationToken { get; set; }
    }

    public class LocationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<LocationCardResponse> LocationCards { get; set; }
    }

    public class LocationCardResponse
    {
        public int Id { get; set; }
        public string CardNo { get; set; }
        public string Name { get; set; }
        public DateTime? OpenedAt { get; set; }
    }

    public class StationOrderItemResponse
    {
        public string CustomerName { get; set; }
        public DateTime? Date { get; set; }

        public string ItemName { get; set; }
        public Decimal Quantity { get; set; }

        public int Status { get; set; }
    }
}