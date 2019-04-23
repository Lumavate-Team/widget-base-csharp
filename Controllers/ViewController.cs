using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    public class ViewController : BaseController
    {
         public ViewController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{urlRef}
        [HttpGet("{instanceId}")]
        [Produces(contentType: "text/html")]
        public IActionResult Render([FromRoute] string ic, [FromRoute] string urlRef, [FromRoute] string instanceId)
        {
            base.init(ic, urlRef);
            var response = new LumavateRequest(Request.Cookies["pwa_jwt"], this.Configuration).Get(Request.Scheme + "://" + Request.Host + "/pwa/v1/widget-instances/" + instanceId);
            System.Console.WriteLine("Tool Instance Data");
            System.Console.WriteLine(response.Result.Value.ToString());
            return View("~/Pages/Home/Index.cshtml");
        }

    }
}