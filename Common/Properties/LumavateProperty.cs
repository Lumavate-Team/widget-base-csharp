using Newtonsoft.Json;
namespace Lumavate.Common.Properties
{
    public static class PropertyTypes {
        public const string TEXT = "text";
        public const string DROPDOWN = "dropdown";
        public const string COLOR = "color";
        public const string IMAGE = "image-upload";
        public const string TRANSLATABLE = "translatableText";
        public const string NUMBER = "numeric";
    }

    public class LumavateProperty
    {
        [JsonProperty("classification")]
        public string classification { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("label")]
        public string label {get; set; }
        [JsonProperty("helpText")]
        public string helpText { get; set; }

        [JsonProperty("default")]
        public string defaultValue { get; set; }
        [JsonProperty("section")]
        public string section { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }

        public LumavateProperty(string c, string s, string n, string l, string p, string dv) {
            this.classification = c;
            this.section = s;
            this.name = n;
            this.label = l;
            this.type = p;
            this.defaultValue = dv;
            this.helpText = "";
        }
    }
}