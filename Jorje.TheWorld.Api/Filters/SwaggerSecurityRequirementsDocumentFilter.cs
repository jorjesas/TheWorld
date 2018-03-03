using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Api.Filters
{

    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Jorje.TheWorld.Api.Filters
    {
        public class SwaggerSecurityRequirementsDocumentFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
        {
            public void Apply(SwaggerDocument document, DocumentFilterContext context)
            {
                document.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[]{ } },
                    { "Basic", new string[]{ } },
                }
            };
            }
        }
    }

}
