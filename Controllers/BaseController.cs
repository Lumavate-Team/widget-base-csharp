using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Lumavate.Common;

namespace Lumavate.Controllers
{
    [Route("{ic}/{urlRef}")]
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected string IntegrationCloud { get; set; }
        protected string UrlRef { get; set; }
        //private readonly LumavateContext _context;
        public EnvironmentConfig Configuration { get; set; }

        public BaseController(IOptions<EnvironmentConfig> config): base() 
        {
            this.Configuration = config.Value;
        }

        public void init(string ic, string urlref) {
            this.IntegrationCloud =ic;
            this.UrlRef = urlref;
        }

        
    }
}