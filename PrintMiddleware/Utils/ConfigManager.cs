using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintMiddleware.Utils
{    
    internal class ConfigManager
    {
        private static string configPath = "config.json";
        private static Dictionary<string, string> config;

        static ConfigManager() 
        { 
            if(File.Exists(configPath))
            {
                config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(configPath));
            } else
            {
                config = new Dictionary<string, string>();
            }            
        }

        public static string Get(string key)
        {
            return config.ContainsKey(key) ? config[key] : null;
        }

        public static void Set(string key, string value)
        {
            config[key] = value;
            File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

    }    
}
