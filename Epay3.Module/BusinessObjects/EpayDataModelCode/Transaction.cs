using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{
    [DefaultClassOptions]
    public partial class Transaction
    {
        public Transaction(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        public void UpdateTotalsAndDate(DateTime? until=null)
        {
            if (Card != null)
            {
                if (until == null)
                {
                    // todo find site transaction belongs to
                    var site = Session.FindObject<Site>(null);
                    var siteNow = DateTime.UtcNow.AddHours(site.Timezone);
                    until = siteNow;
                }

                var oldBalance = Card.Transactions
                    .Where(t=>t!=this)
                    .SelectMany(tr => tr.TransactionDetails)
                    .Where(td =>
                        (td.ValidFrom == null || td.ValidFrom <= until)
                        && (td.ValidTo == null || td.ValidTo >= until))
                    .Select(td => td.Total)
                    .Sum();

                TotalCharges = TransactionDetails.Where(td => td.Item.Type == ETransactionType.Charge)
                    .Select(td => td.Total)
                    .Sum();

                TotalSales = TransactionDetails.Where(td => td.Item.Type == ETransactionType.Withdraw)
                    .Select(td => td.Total)
                    .Sum();

                var amount = TotalSales + TotalCharges;

                this.OldBalance = oldBalance;
                this.NewBalance = OldBalance + amount;
                this.Amount = amount;
                this.Date = (DateTime) until;
            }
        }
    }
}