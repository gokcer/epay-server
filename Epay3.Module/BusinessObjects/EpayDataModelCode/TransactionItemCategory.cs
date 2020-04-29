using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{
    [DefaultClassOptions]
    public partial class TransactionItemCategory
    {
        public TransactionItemCategory(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
