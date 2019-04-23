using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Lumavate.Models;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    public class HealthController : WidgetController
    {
         public HealthController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{widgetType}/health
        [HttpGet("health")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<string>> GetHealth([FromRoute] string ic, [FromRoute] string widgetType)
        {
            base.init(ic, widgetType);
            System.Console.WriteLine("Integration Cloud:" + this.integrationCloud);
            System.Console.WriteLine("Widget Type:" + this.urlRef);
            return new string[] { "status: OK" };
        }

    }
}