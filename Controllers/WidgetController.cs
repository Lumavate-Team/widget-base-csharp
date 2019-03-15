using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Lumavate.Models;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    [Route("{ic}/{widgetType}")]
    [ApiController]
    public abstract class WidgetController : Controller
    {
        protected string integrationCloud { get; set; }
        protected string urlRef { get; set; }
        //private readonly LumavateContext _context;
        public EnvironmentConfig configuration { get; set; }

        public WidgetController(IOptions<EnvironmentConfig> config): base() 
        {
            this.configuration = config.Value;
        }

        public void init(string ic, string widgetType) {
            this.integrationCloud =ic;
            this.urlRef = widgetType;
        }

        
    }
}