using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    public class HealthController : BaseController
    {
         public HealthController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{urlRef}/health
        [HttpGet("health")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<string>> GetHealth([FromRoute] string ic, [FromRoute] string urlRef)
        {
            base.init(ic, urlRef);
            System.Console.WriteLine("Integration Cloud:" + this.IntegrationCloud);
            System.Console.WriteLine("URL Reference:" + this.UrlRef);
            return new string[] { "status: OK" };
        }

    }
}