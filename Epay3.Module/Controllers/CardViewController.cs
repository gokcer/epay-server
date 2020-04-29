using System;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using Epay3.Module.BusinessObjects;
using Epay3.Module.BusinessObjects.EpayDataModel;

namespace Epay3.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CardViewController : ViewController
    {
        public CardViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void paCharge_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var npObjectSpace = Application.CreateObjectSpace(typeof(NPNewDailyCharge));
            var npNewDailyCharge = npObjectSpace.CreateObject<NPNewDailyCharge>();
            npNewDailyCharge.Quantity = 1;

            var detailView = Application.CreateDetailView(npObjectSpace, npNewDailyCharge);
            detailView.ViewEditMode = ViewEditMode.Edit;
            e.View = detailView;
        }

        private void paCharge_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            NPNewDailyCharge newCharge = (NPNewDailyCharge) e.PopupWindowViewCurrentObject;
            var transactionItem = newCharge.Item;
            var quantity = newCharge.Quantity;
            var timezone = ObjectSpace.GetObjects<Site>().First().Timezone;
            var siteNow = DateTime.UtcNow.AddHours(timezone);

            foreach (Card card in View.SelectedObjects)
            {
                // create transaction
                var transaction = ObjectSpace.CreateObject<Transaction>();
                transaction.Card = card;
                transaction.ProductType = transactionItem.ProductType;
                transaction.Date = siteNow;
                if (newCharge.Calendar != null)
                {
                    foreach (var calendarDay in newCharge.Calendar.CalendarDays)
                    {
                        var transactionDetail = ObjectSpace.CreateObject<TransactionDetail>();
                        {
                            transactionDetail.ValidFrom = calendarDay.Date.Date;
                            transactionDetail.ValidTo = calendarDay.Date.AddDays(1).AddSeconds(-1);
                            transactionDetail.Quantity = quantity;
                            transactionDetail.Total =
                                transactionItem.Price == 0 ? quantity : transactionItem.Price * quantity;
                            transactionDetail.Item = ObjectSpace.GetObject(transactionItem);
                        }

                        transaction.TransactionDetails.Add(transactionDetail);
                    }
                }
                else
                {
                    // charge normal item
                    var transactionDetail = ObjectSpace.CreateObject<TransactionDetail>();
                    {
                        transactionDetail.Quantity = quantity;
                        transactionDetail.Total =
                            transactionItem.Price == 0 ? quantity : transactionItem.Price * quantity;
                        transactionDetail.Item = ObjectSpace.GetObject(transactionItem);
                    }
                    transaction.TransactionDetails.Add(transactionDetail);
                }
                transaction.UpdateTotalsAndDate(siteNow);
            }

            ObjectSpace.CommitChanges();
        }

        private void singleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Id)
            {
                case "date":

                    break;
                case "calendar":
                    break;
            }
        }

        private void paInvalidate_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            NPInvalidateCard invalidation = (NPInvalidateCard) e.PopupWindowViewCurrentObject;


            Debug.Assert(View.SelectedObjects.Count == 1, "Card invalidation should be executed for single record");
            Card card = (Card) View.SelectedObjects[0];
            // create transaction
            var transaction = ObjectSpace.CreateObject<Transaction>();
            transaction.Card = card;

            transaction.ProductType = EProductType.Money;

            //
            var tiYukleme = (ObjectSpace.GetObjectsQuery<TransactionItem>()
                .Where(ti => ti.Active && ti.ProductType == EProductType.Money && ti.Type == ETransactionType.Charge)).First();

            var transactionDetail = ObjectSpace.CreateObject<TransactionDetail>();
            var refundAmount = -1 * invalidation.RefundAmount;
            transactionDetail.Item = tiYukleme;
            transactionDetail.Quantity = refundAmount;
            transactionDetail.Total = refundAmount;
            transaction.TransactionDetails.Add(transactionDetail);

            if (card.Returnable)
            {
                var transactionDetailDeposit = ObjectSpace.CreateObject<TransactionDetail>();
                transactionDetailDeposit.Item = ObjectSpace.GetObjectsQuery<TransactionItem>().First(ti=>ti.SpecialCode=="DEPOSIT");
                transactionDetailDeposit.Quantity = -1;
                transactionDetailDeposit.Total = invalidation.DepositAmount;
                transaction.TransactionDetails.Add(transactionDetailDeposit);
            }

            transaction.UpdateTotalsAndDate();

            card.CardNo = $"Eski kart - {card.CardNo} - {DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}";

            card.Active = false;

            ObjectSpace.CommitChanges();
        }

        private void paInvalidate_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var npObjectSpace = Application.CreateObjectSpace(typeof(NPInvalidateCard));
            var npInvalidateCard = npObjectSpace.CreateObject<NPInvalidateCard>();

            Debug.Assert(View.SelectedObjects.Count == 1, "Card invalidation should be executed for single record");
            Card card = (Card) View.SelectedObjects[0];

            npInvalidateCard.CurrentBalance = card.Transactions.Sum(t => t.Amount);

            decimal depositAmount = 0;

            if (card.Returnable)
            {
                depositAmount = -1 * npObjectSpace.GetObjectsQuery<TransactionDetail>().First(td =>
                    td.Transaction.Card.Oid == card.Oid && td.Item.SpecialCode=="DEPOSIT").Total;
            }

            npInvalidateCard.DepositAmount = depositAmount;
            npInvalidateCard.RefundAmount = npInvalidateCard.CurrentBalance + npInvalidateCard.DepositAmount;

            var detailView = Application.CreateDetailView(npObjectSpace, npInvalidateCard);
            detailView.ViewEditMode = ViewEditMode.Edit;
            e.View = detailView;
        }
    }
}