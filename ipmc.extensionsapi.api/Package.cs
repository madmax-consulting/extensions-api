using System;
using Newtonsoft.Json;

namespace ipmc.extensionsapi.common
{
    public class Package
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}