using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lumavate.Common;
using Lumavate.Common.Properties;

namespace Lumavate.Controllers
{
    public class PropertyController : BaseController
    {
        private const string Path = "studio-properties.json";

        public PropertyController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{urlRef}/discover/properties
        [HttpGet("discover/properties")]
        [Produces("application/json")]
        public ActionResult<ApiResponse> GetProperties([FromRoute] string ic, [FromRoute] string urlRef)
        {
            base.init(ic, urlRef);
            System.Console.WriteLine("Integration Cloud:" + this.IntegrationCloud);
            System.Console.WriteLine("URL Reference:" + this.UrlRef);

            //Read in Properties from the studio-properties.json file
            string json = System.IO.File.ReadAllText(Path);
            var properties = JsonConvert.DeserializeObject<List<LumavateProperty>>(json);

            // var properties = new List<LumavateProperty>();
            
            // properties.Add(new LumavateProperty("Header","Properties","backgroundColor","Background Color",PropertyTypes.COLOR,"#000000"));
            // properties.Add(new LumavateProperty("Body","Properties","bodyColor","Background Color",PropertyTypes.COLOR,"#000000"));
            // properties.Add(new LumavateProperty("Body","Properties","sampleText","Sample Text",PropertyTypes.TEXT,""));
            // properties.Add(new LumavateProperty("Body","Properties","sampleText2","Sample Text 2",PropertyTypes.COLOR,"#a2a2a2"));
            
            return new ApiResponse(new LumavatePayload(properties));
        }

    }
}