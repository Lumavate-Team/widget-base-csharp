using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace Lumavate.Common
{
    public class LumavateRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        public EnvironmentConfig configuration { get; set; }

        public LumavateRequest(string token, EnvironmentConfig config) 
        {
            this.Token = token;
            this.configuration = config;
        }

        public async Task<OkObjectResult> Get(string url) {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                
                try 
                {
                    var signer = new Signer(this.configuration);
                    client.DefaultRequestHeaders.Add("Authorization","Bearer " + this.Token);
                    List<KeyValuePair<string,string>> signed_url = signer.GetSignature("GET",url,null);
                    result = await client.GetStringAsync(signed_url.First(z => z.Key == "s-url").Value.ToString());
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
        private string privateKey { get; set; }
        private string publicKey { get; set; }

        public Signer(EnvironmentConfig config) 
        {
            this.publicKey = config.PUBLIC_KEY;
            this.privateKey = config.PRIVATE_KEY;
        }

        static string GetMd5Hash(MD5 md5Hash, byte[] input)
        {
            var buffer = new byte[0];
            if (input != null)
                buffer = input;

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(buffer);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public List<KeyValuePair<string,string>> GetSignature(string method, string urlToSign, byte[] body, string forced_nonce = "", string forced_time = "") 
        {
            string body_md5 = "";

            //Calculate a request signature based on given context
            using (MD5 md5Hash = MD5.Create())
            {
                body_md5 = GetMd5Hash(md5Hash, body);
            }

            // Parse URI, and grab everything except the query string.
            var uri = new Uri(urlToSign);
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);

            // Grab just the query string part
            var parsed_query = QueryHelpers.ParseQuery(uri.Query);

            // Convert the StringValues into a list of KeyValue Pairs to make it easier to manipulate
            var query_items = parsed_query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            // At this point you can remove items if you want
            query_items.RemoveAll(x => x.Key == "s-key" || x.Key == "s-time" || x.Key == "s-hash" || x.Key == "s-signature" || x.Key == "s-nonce"); // Remove specific value for key
            
            List<KeyValuePair<string,string>> additional_query = new List<KeyValuePair<string, string>>();
            additional_query.Add(new KeyValuePair<string, string>("s-hash", body_md5));
            additional_query.Add(new KeyValuePair<string, string>("s-key", this.publicKey));
            
            //For the sake of checking a signature, allow the time & nonce value to be
            //passed directly in rather than calculated
            if (!String.IsNullOrWhiteSpace(forced_time))
                additional_query.Add(new KeyValuePair<string, string>("s-time", forced_time));
            else
                additional_query.Add(new KeyValuePair<string, string>("s-time", ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString()));

            if (!String.IsNullOrWhiteSpace(forced_nonce))
                additional_query.Add(new KeyValuePair<string, string>("s-nonce", forced_nonce));
            else {
                var random = new System.Random();
                additional_query.Add(new KeyValuePair<string, string>("s-nonce", random.Next(0, 1000000000).ToString()));
            }
            
            List<KeyValuePair<string,string>> full_parms = new List<KeyValuePair<string, string>>();
            foreach(var item in query_items)
                full_parms.Add(new KeyValuePair<string,string>(item.Key,item.Value));

            foreach(var item in additional_query)
                full_parms.Add(new KeyValuePair<string,string>(item.Key,item.Value));

            //Need to sort
            full_parms.Sort((a,b) => (a.Key.CompareTo(b.Key)));
            var qb = new QueryBuilder(full_parms);

            var key = $"{method.ToLower()}\n{(uri.AbsolutePath.ToString().ToLower())}\n{qb.ToQueryString().ToString().Replace("?","")}\n{additional_query.First(z => z.Key == "s-nonce").Value.ToString()}";
 
            //del additional_query['s-hash']
            additional_query.Remove(new KeyValuePair<string,string>("s-hash", body_md5));

            // Create a UTF-8 encoding.
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] keyBytes = utf8.GetBytes(this.privateKey);
            Byte[] msgBytes = utf8.GetBytes(key);
            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes)) {
                hashBytes = hash.ComputeHash(msgBytes);
            }

            var signature = Convert.ToBase64String(hashBytes);

            additional_query.Add(new KeyValuePair<string,string>("s-signature",signature));

            var additional_query_string = new QueryBuilder(additional_query).ToQueryString().ToString();
            var signed_url = urlToSign;

            if (signed_url.IndexOf("?") > 0) {
                signed_url += "&" + additional_query_string.Replace("?","");
            }
            else {
                signed_url += additional_query_string;
            }

            additional_query.Add(new KeyValuePair<string,string>("s-url",signed_url));
            
            //System.Console.WriteLine("\nPublicKey => " + this.publicKey);
            //System.Console.WriteLine("\nHash Content =>\n" + key.ToString());
            //System.Console.WriteLine("\nSignature => " + signature.ToString());
            //System.Console.WriteLine("\nSigned URL => " + signed_url.ToString()+"\n");
            return additional_query;
        }
    }
}