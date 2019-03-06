using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lumavate.Common
{
    public class LumavateRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        public LumavateRequest(string token) {
            this.Token = token;
        }

        public async Task<OkObjectResult> Get(string url) {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                
                try 
                {
                    var signer = new Signer();
                    client.DefaultRequestHeaders.Add("Authorization","Bearer " + this.Token);
                    result = await client.GetStringAsync(signer.GetSignature("GET",url, null));
                } 
                catch(HttpRequestException e) 
                {
                    System.Console.WriteLine("ERROR: {0}",e.Message);
                }
            }

            return new OkObjectResult(result);
        }
    }

    public class Signer
    {
        public Signer() {

        }

        public string GetSignature(string method, string urlToSign, byte[] body) {
            return urlToSign;
        }
    }
}