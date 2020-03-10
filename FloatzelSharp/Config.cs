using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp
{
    internal class Config
    {
        // stolen from kekbot...again
        // ah well
        [JsonProperty("token")]
        public string Token { get; private set; }

        private static Config _instance;

        // lifted from kekbot..?
        public static async Task<Config> Get()
        {
            if (_instance == null)
            {
                using var fs = File.OpenRead("../../../../config.json");
                using var sr = new StreamReader(fs, new UTF8Encoding(false));
                return _instance = JsonConvert.DeserializeObject<Config>(await sr.ReadToEndAsync());
            }
            else return _instance;
        }
    }
}
