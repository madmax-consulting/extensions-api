using Newtonsoft.Json;

namespace ipmc.extensionsapi.common
{
    public class Status
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("isPaused")]
        public bool IsPaused { get; set; }
    }
}