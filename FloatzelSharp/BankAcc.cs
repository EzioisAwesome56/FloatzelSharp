using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp {
    public class BankAcc {

        [JsonProperty]
        public string uid { get; private set; }
        [JsonProperty]
        public int bal { get; private set; }
    }
}
