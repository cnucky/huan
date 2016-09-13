using System.Collections.Generic;
using System.IO;

namespace Common {
    public static class ConfigHelper {
        public static void InitConfig(string filename) {
            kvs.Clear();
            StreamReader srReader = new StreamReader(filename, true);
            string line = "";
            while ((line = srReader.ReadLine()) != null) {
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                if (line.Contains("=")) {
                    string k1 = line.Split('=')[0].Trim();
                    string v1 = line.Split('=')[1].Trim();
                    if (!kvs.ContainsKey(k1)) {
                        kvs.Add(k1, v1);
                    }
                }
            }
            srReader.Close();
        }
        static Dictionary<string, string> kvs = new Dictionary<string, string>();
        public static string GetValue(string key) {
            InitConfig("config//sysConfig.txt");
            if (kvs.ContainsKey(key)) {
                return kvs[key];
            } else {
                return "";
            }
        }

        public static bool GetBoolValue(string p)
        {
            return bool.Parse(GetValue(p));
        }

        public static int GetIntValue(string p) {
            return int.Parse(GetValue(p));
        }
    }
}