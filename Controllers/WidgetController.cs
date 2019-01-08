using Microsoft.AspNetCore.Mvc;
using Lumavate.Models;

namespace Lumavate.Controllers
{
    [Route("{ic}/{widgetType}")]
    [ApiController]
    public abstract class WidgetController : Controller
    {
        protected string integrationCloud { get; set; }
        protected string urlRef { get; set; }

        public WidgetController(): base() {}

        public void init(string ic, string widgetType) {
            this.integrationCloud =ic;
            this.urlRef = widgetType;
        }

        
    }
}