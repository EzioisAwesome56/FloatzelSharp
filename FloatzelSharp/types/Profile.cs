﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.types {
    public class Profile {

        [JsonProperty]
        public string uid { get; set; }
        [JsonProperty]
        public int bal { get; set; }
        [JsonProperty]
        public double loantime { get; set; }
        [JsonProperty]
        public bool bloan { get; set; }
        [JsonProperty]
        public int[] boxes { get; set; }
        [JsonProperty]
        public int[] stock { get; set; }
        [JsonProperty]
        public bool admin { get; set; }

    }
}
