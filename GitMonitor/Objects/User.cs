using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class User
    {
        private dynamic id;
        private dynamic login;

        public User(dynamic id, dynamic login)
        {
            this.id = id;
            this.login = login;
        }

        public string ID { get { return id; } set { } }
        public string Name { get { return login; } set { } }
    }
}
