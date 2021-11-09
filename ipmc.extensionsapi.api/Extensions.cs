using System.Collections.Generic;
using Newtonsoft.Json;

namespace ipmc.extensionsapi.common
{
    public class Extensions
    {
        [JsonProperty("extensionId")]
        public string ExtensionId { get; set; }

        [JsonProperty("extensionType")]
        public string ExtensionType { get; set; }

        [JsonProperty("assemblyName")]
        public string AssemblyName { get; set; }

        [JsonProperty("assemblyType")]
        public string AssemblyType { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("package")]
        public Package Package { get; set; }

        [JsonProperty("settings")]
        public List<ExtensionSetting> Settings { get; set; }
    }

    public class ExtensionSetting
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
