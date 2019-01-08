using Newtonsoft.Json;
namespace Lumavate.Common
{
    public class ApiResponse
    {
        [JsonProperty("payload")]
        public LumavatePayload Payload { get; set; }

        public ApiResponse(LumavatePayload payload) {
            this.Payload = payload;
        }
    }

    public class LumavatePayload
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        public LumavatePayload(object data) {
            this.Data = data;
        }
    }
}