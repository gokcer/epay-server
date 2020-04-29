using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SaasKit.Multitenancy;

namespace Epay3.Api.Tenancy
{
    public class AppTenantResolver:ITenantResolver<AppTenant>
    {
        private readonly IConfiguration _configuration;

        ILog logger = LogManager.GetLogger(typeof(AppTenantResolver));

        public AppTenantResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<TenantContext<AppTenant>> ResolveAsync(HttpContext context)
        {
            var client = "";
            var contextRequest = context.Request;

            if (contextRequest.Query.ContainsKey("client"))
            {
                client = contextRequest.Query["client"];
            } 
            else if (contextRequest.Headers.ContainsKey("client"))
            {
                client = contextRequest.Headers["client"].First();
            }

            client = client.ToLowerInvariant();

            var appTenant = new AppTenant();
            appTenant.Client = client; 
            var confStr = _configuration.GetValue<string>("EPAY_API_CONNECTION_STRING");
           
            appTenant.ConnectionString = confStr.Replace("TENANT_SCHEMA", "epay3." + client);

            return Task.FromResult(new TenantContext<AppTenant>(appTenant));
        }

        
    }
}
