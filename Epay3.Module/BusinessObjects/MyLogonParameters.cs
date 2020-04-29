using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;

namespace Epay3.Module.BusinessObjects
{
    [DomainComponent]
    [XafDisplayName("NFC Pay Login")]
    public class MyLogonParameters:AuthenticationStandardLogonParameters,ICustomObjectSerialize
    {
        public string Client { get; set; }
        public void ReadPropertyValues(SettingsStorage storage)
        {
            UserName = storage.LoadOption("", "UserName");
            Client = storage.LoadOption("", "Client");
        }
        public void WritePropertyValues(SettingsStorage storage)
        {
            storage.SaveOption("", "UserName", UserName);
            storage.SaveOption("", "Client", Client);
        }
    }
}
