using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp {
    public class Profile {

        [JsonProperty]
        public string uid { get; set; }
        [JsonProperty]
        public int bal { get; set; }
        [JsonProperty]
        public double loantime { get; set; }
    }
}
