using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Epay3.Api.Tenancy
{
    // https://stackoverflow.com/a/49035476/2346618
    public class SwaggerSecurityRequirementsDocumentFilter:IDocumentFilter
    {
        public void Apply(SwaggerDocument document, DocumentFilterContext context)
        {
            document.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "client", new string[]{ } },
                }
            };
        }
        
    }
}