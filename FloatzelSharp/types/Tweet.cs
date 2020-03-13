using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.types {
    class Tweet {

        [JsonProperty("tid")]
        public string tid { get; set; }

        [JsonProperty("txt")]
        public string txt { get; set; }
    }
}
