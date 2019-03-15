using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lumavate.Models;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    public class RenderController : WidgetController
    {
         public RenderController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{widgetType}
        [HttpGet("{instanceId}")]
        [Produces(contentType: "text/html")]
        public IActionResult Render([FromRoute] string ic, [FromRoute] string widgetType, [FromRoute] string instanceId)
        {
            base.init(ic, widgetType);
            var response = new LumavateRequest(Request.Cookies["pwa_jwt"], this.configuration).Get(Request.Scheme + "://" + Request.Host + "/pwa/v1/widget-instances/" + instanceId);
            System.Console.WriteLine("Widget Instance");
            System.Console.WriteLine(response.Result.Value.ToString());
            return View("~/Pages/Home/Index.cshtml");
        }

    }
}