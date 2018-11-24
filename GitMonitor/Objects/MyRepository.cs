using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class MyRepository
    {
        private dynamic id;
        private dynamic login;
        private dynamic owner;

        public MyRepository(dynamic id, dynamic login, dynamic owner)
        {
            this.id = id;
            this.login = login;
            this.owner = owner;
        }

        public string ID { get { return id; } set { } }
        public string Name { get { return login; } set { } }
        public string Owner { get { return owner; } set { } }
    }
}
