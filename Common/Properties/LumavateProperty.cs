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
        [JsonProperty("classification", Required = Required.Always)]
        public string classification { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        public string type { get; set; }
        [JsonProperty("label", Required = Required.AllowNull)]
        public string label {get; set; }
        [JsonProperty("helpText")]
        public string helpText { get; set; }

        [JsonProperty("default")]
        public string defaultValue { get; set; }
        [JsonProperty("section", Required = Required.AllowNull)]
        public string section { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string name { get; set; }
        [JsonProperty("options")]
        public Options options { get; set; }

        public LumavateProperty(string c, string s, string n, string l, string p, string dv) {
            this.classification = c;
            this.section = s;
            this.name = n;
            this.label = l;
            this.type = p;
            this.defaultValue = dv;
            this.helpText = "";
            this.options = new Options();
        }
    }

    public class Options
    {
        [JsonProperty("rows")]
        public int rows { get; set; }

        [JsonProperty("min")]
        public int min { get; set; }

        [JsonProperty("max")]
        public int max { get; set; }
    }
}