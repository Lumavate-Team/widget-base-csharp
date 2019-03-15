using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumavate.Models;
using Lumavate.Common;
using Lumavate.Common.Properties;

namespace Lumavate.Controllers
{
    public class PropertyController : WidgetController
    {
         public PropertyController(IOptions<EnvironmentConfig> config):base(config) { }

        // GET {ic}/{widgetType}/discover/properties
        [HttpGet("discover/properties")]
        [Produces("application/json")]
        public ActionResult<ApiResponse> GetProperties([FromRoute] string ic, [FromRoute] string widgetType)
        {
            base.init(ic, widgetType);
            System.Console.WriteLine("Integration Cloud:" + this.integrationCloud);
            System.Console.WriteLine("Widget Type:" + this.urlRef);

            var properties = new List<LumavateProperty>();
            
            properties.Add(new LumavateProperty("Header","Properties","backgroundColor","Background Color",PropertyTypes.COLOR,"#000000"));
            properties.Add(new LumavateProperty("Body","Properties","bodyColor","Background Color",PropertyTypes.COLOR,"#000000"));
            properties.Add(new LumavateProperty("Body","Properties","sampleText","Sample Text",PropertyTypes.TEXT,""));
            properties.Add(new LumavateProperty("Body","Properties","sampleText2","Sample Text 2",PropertyTypes.COLOR,"#a2a2a2"));
            
            //System.Console.WriteLine("Properties:" + properties.ToString());

            //_context.Properties = properties;
            
            return new ApiResponse(new LumavatePayload(properties));
        }

    }
}