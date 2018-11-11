using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class CredentialStorage
    {
        private static Dictionary<string, string> credentials = new Dictionary<string, string>();

        public static void addItem(string user, string token)
        {
            credentials[user] = token;
        }

        public static void remove(string user)
        {
            credentials.Remove(user);
        }

        public static string get(string user)
        {
            return credentials[user];
        }
    }
}
