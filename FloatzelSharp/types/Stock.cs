using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.types {
    class Stock {

        [JsonProperty("sid")]
        public string sid { get; set; }
        [JsonProperty("diff")]
        public int diff { get; set; }
        [JsonProperty("price")]
        public int price { get; set; }
        [JsonProperty("units")]
        public int units { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
