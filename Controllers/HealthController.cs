using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lumavate.Models;

namespace Lumavate.Controllers
{
    public class HealthController : WidgetController
    {
         public HealthController():base() { }

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