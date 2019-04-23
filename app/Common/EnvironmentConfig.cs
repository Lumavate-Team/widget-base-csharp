using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lumavate.Common
{
    public class EnvironmentConfig
    {   
        public string PUBLIC_KEY { get; set; }
        public string PRIVATE_KEY { get; set; }
        public string PROTO { get; set; }
        
    }
}