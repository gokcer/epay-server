using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Epay3.Api.Models;
using Epay3.Api.Models.Api;
using Epay3.Api.Models.Integration;
using Epay3.Api.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Card = Epay3.Api.Models.Integration.Card;
using Product = Epay3.Api.Models.Integration.Product;
using Transaction = Epay3.Api.Models.Integration.Transaction;

namespace Epay3.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/integration")]
    public class IntegrationController : Controller
    {
        private readonly Epay3Context _db;
        private readonly AppTenant _appTenant;

        public IntegrationController(Epay3Context db, AppTenant appTenant)
        {
            _db = db;
            _appTenant = appTenant;
        }

        [HttpGet]
        [Route("Staff")]
        public RestResult<IEnumerable<StaffResponse>> Staff([FromHeader] string authToken, DateTime balanceDate)
        {
            Authenticate(authToken);
            return new RestResult<IEnumerable<StaffResponse>>(_db.Customer.Include(c => c.Card)
                .Include(c => c.ObjectTypeNavigation)
                .Include(c => c.Transaction).Select(c => new StaffResponse()
                {
                    Id = c.Oid,
                    Name = c.Name,
                    CitizenshipNumber = c.CitizenshipNumber,
                    Email = c.Email,
                    Cards = c.Card.Select(card => new Card()
                    {
                        Id = card.Oid,
                        Active = card.Active,
                        CardNo = card.CardNo,
                        Balance = card.Transaction.Where(t => t.Date < balanceDate)
                            .Select(t => t.TotalCharges.Value + t.TotalSales.Value).Sum()
                    }),
                    Phone = c.Phone,
                    NotificationToken = c.NotificationToken,
                    Team = c.Team,
                    Employee = c.ObjectTypeNavigation.TypeName.EndsWith(".Employee")
                }));
        }

        private Site Authenticate(string authToken)
        {
            var site = _db.Site.FirstOrDefault(s => s.RegistrationCode == authToken);
            if (site == null) throw new SecurityException();
            return site;
        }

        [HttpGet]
        [Route("Transactions")]
        public RestResult<IEnumerable<Transaction>> Transactions([FromHeader] string authToken, DateTime start,
            DateTime end)
        {
            Authenticate(authToken);
            var list = _db.TransactionDetail
                .Where(td => (td.TransactionNavigation.Date > start && td.TransactionNavigation.Date < end)
                || (td.TransactionNavigation.DateEffective > start && td.TransactionNavigation.DateEffective < end))
                .Select(td => new Transaction()
                {
                    Id = td.TransactionNavigation.Oid,
                    IdTransactionDetail = td.Oid,
                    Date = td.TransactionNavigation.Date,
                    DateEffective = td.TransactionNavigation.DateEffective??td.TransactionNavigation.Date,
                    Name = td.ItemNavigation.Name,
                    Price = td.ItemNavigation.Price,
                    Quantity = td.Quantity,
                    Total = td.Total,
                    Type = td.ItemNavigation.Type,
                    ProductId = td.ItemNavigation.Oid,
                    ProductType = td.ItemNavigation.ProductType,
                    CardNo = td.TransactionNavigation.CardNavigation.CardNo,
                    CustomerId = td.TransactionNavigation.CardNavigation.CustomerNavigation.Oid,
                    CustomerName = td.TransactionNavigation.CardNavigation.CustomerNavigation.Name,
                    CustomerPhone = td.TransactionNavigation.CardNavigation.CustomerNavigation.Phone,
                    Employee = td.TransactionNavigation.DeviceLoginNavigation == null
                        ? null
                        : td.TransactionNavigation.DeviceLoginNavigation.EmployeeNavigation.Name,
                    EmployeeId = td.TransactionNavigation.DeviceLoginNavigation == null
                        ? (int?) null
                        : td.TransactionNavigation.DeviceLoginNavigation.EmployeeNavigation.Oid,
                    IsCancel =  td.TransactionNavigation.IsCancel
                }).ToList();

            return new RestResult<IEnumerable<Transaction>>(list);
        }

        [HttpGet]
        [Route("Products")]
        public RestResult<IEnumerable<Product>> Products([FromHeader] string authToken)
        {
            Authenticate(authToken);
            return new RestResult<IEnumerable<Product>>(_db.TransactionItem.Select(t => new Product()
            {
                Id = t.Oid,
                Name = t.Name,
                Price = t.Price,
                Active = t.Active,
                ProductType = t.ProductType,
                Type = t.Type
            }));
        }

        [HttpPut]
        [Route("SetCardLimit")]
        public RestResult<Card> SetCardLimit([FromHeader] string authToken, string cardNo, int limit)
        {
            Authenticate(authToken);

            var card = _db.Card.FirstOrDefault(c => c.CardNo == cardNo);
            if (card == null) return new RestResult<Card>(false, "Card not found for no " + cardNo, null);

            card.MinimumBalanceLimit = limit;

            _db.SaveChanges();

            return new RestResult<Card>(new Card()
            {
                Active = card.Active,
                CardNo = card.CardNo,
                Id = card.Oid,
            });
        }

        [HttpPut]
        [Route("ImportCards")]
        public RestResult<object> ImportCards([FromHeader] string authToken, [FromBody] List<StaffResponse> staff)
        {
            var site = Authenticate(authToken);

            // filter out employees
            staff = staff.Where(s => !s.Employee).ToList();

            var customer = _db.Customer.First();
            var sourceCardNos = staff.SelectMany(s => s.Cards).Where(c => (bool) c.Active).Select(c => c.CardNo);
            var destinationCardNos = _db.Card.Select(c => c.CardNo).ToList();
            var cardsToImport = sourceCardNos.Except(destinationCardNos);

            var staffToImport = staff.Where(s => cardsToImport.Intersect(s.Cards.Select(c => c.CardNo)).Count() > 0)
                .ToList();

            // todo check if card belongs to personnel or customer
            //F39C1E7B
            var customerPhones = _db.Customer.Select(c => c.Phone).ToList();
            foreach (var record in staffToImport)
            {
                Customer relatedCustomer;

                var customerExists = customerPhones.Contains(record.Phone);
                if (!customerExists)
                {
                    // Customer does not exists. Create a new customer
                    relatedCustomer = new Customer()
                    {
                        CitizenshipNumber = record.CitizenshipNumber,
                        Name = record.Name,
                        Email = record.Email, // user name and password are not used for customers for now
                        Phone = record.Phone,
                        NotificationToken = record.NotificationToken,
                        Team = record.Team
                    };
                    _db.Customer.Add(relatedCustomer);
                }
                else
                {
                    // Customer does exists. Use it and add new card
                    relatedCustomer = _db.Customer.Include(c=>c.Card).First(c => c.Phone == record.Phone);
                }

                // create cards which does not exist at customer
                var relatedCustomerExistingCardNumbers = relatedCustomer.Card.Select(c=>c.CardNo).ToList();

                foreach (var recordCard in record.Cards)
                {
                    if (!relatedCustomerExistingCardNumbers.Contains(recordCard.CardNo))
                    {
                        // create card
                        var newCard = new Models.Card()
                        {
                            Active = recordCard.Active,
                            CardNo = recordCard.CardNo,
                        };
                        // add to customer
                        relatedCustomer.Card.Add(newCard);
                    }
                }
            }

            _db.SaveChanges();
            return new RestResult<object>(staffToImport);
        }

        [HttpGet]
        [Route("Test")]
        public string Test()
        {
            return "OK";
        }

[HttpPost]
        [Route("AddTransaction")]
        public RestResult<Card> AddTransaction([FromHeader] string authToken, string cardNo,
            string transactionItemSpecialCode,
            decimal amount)
        {
            var site = Authenticate(authToken);

            //
            // Find card and transactionItem
            //
            var card = _db.Card.FirstOrDefault(c => c.CardNo == cardNo);
            if (card == null) return new RestResult<Card>(false, "Card not found for no " + cardNo, null);

            var transactionItem =
                _db.TransactionItem.FirstOrDefault(ti => ti.SpecialCode == transactionItemSpecialCode);
            if (transactionItem == null)
                return new RestResult<Card>(false,
                    "Transaction item not found for special code " + transactionItemSpecialCode, null);

            //
            // create transactionDetail and transaction
            //
            var transaction =
                new Models.Transaction() {CardNavigation = card, ProductType = transactionItem.ProductType};

            var total = (transactionItem.Price == 0) ? amount : (transactionItem.Price * amount);

            var transactionDetail = new TransactionDetail()
            {
                ItemNavigation = transactionItem,
                Quantity = amount,
                Total = total,
            };
            transaction.TransactionDetail.Add(transactionDetail);

            //
            // update totals for transaction
            //
            var siteNow = DateTime.Now.AddHours(site.Timezone.Value);

            var transactionDetailsOfCard =
                _db.TransactionDetail.Where(td => td.TransactionNavigation.CardNavigation == card);
            var oldBalance = transactionDetailsOfCard.Where(td => (td.ValidFrom == null || td.ValidFrom <= siteNow)
                                                                  && (td.ValidTo == null || td.ValidTo >= siteNow))
                .Sum(td => td.Total);
            var totalCharges = transaction.TotalSales = transaction.TransactionDetail
                .Where(td => td.ItemNavigation.Type == 2).Select(td => td.Total).Sum();
            var totalSales = transaction.TotalSales = transaction.TransactionDetail
                .Where(td => td.ItemNavigation.Type == 1).Select(td => td.Total).Sum();

            var transactionAmount = totalCharges + totalSales;
            var newBalance = oldBalance + transactionAmount;
            transaction.OldBalance = oldBalance;
            transaction.NewBalance = newBalance;
            transaction.Amount = transactionAmount;
            transaction.Date = siteNow;

            _db.Transaction.Add(transaction);
            _db.SaveChanges();

            //
            // retrieve card object and return values
            //
            card = _db.Card.First(c => c.Oid == card.Oid);

            return new RestResult<Card>(new Card()
            {
                Active = card.Active,
                CardNo = card.CardNo,
                Id = card.Oid,
                Balance = newBalance
            });
        }
    }
}