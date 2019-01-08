using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lumavate.Models;

namespace Lumavate.Controllers
{
    public class RenderController : WidgetController
    {
         public RenderController():base() { }

        // GET {ic}/{widgetType}
        [HttpGet("{instanceId}")]
        [Produces(contentType: "text/html")]
        public IActionResult Render([FromRoute] string ic, [FromRoute] string widgetType, [FromRoute] string instanceId)
        {
            base.init(ic, widgetType);
            System.Console.WriteLine("Integration Cloud:" + this.integrationCloud);
            System.Console.WriteLine("Widget Type:" + this.urlRef);
            System.Console.WriteLine("Instance Id:" + instanceId.ToString());
            return View("~/Pages/Home/Index.cshtml");
        }

    }
}