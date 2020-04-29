using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace Epay3.Module.BusinessObjects.EpayDataModel
{

    [DefaultClassOptions]
    public partial class Order
    {
        public Order(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
