using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class ConfigManager
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        public static void addItem(string key, string value)
        {
            dict[key] = value;
        }

        public static string get(string key)
        {
            return dict[key];
        }
    }
}
