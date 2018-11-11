using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    public class Organization
    {
        private dynamic id;
        private dynamic login;
        private dynamic description;

        public Organization(dynamic id, dynamic login, dynamic description)
        {
            this.id = id;
            this.login = login;
            this.description = description;
        }

        public string ID { get { return id; } set { } }
        public string Name { get { return login; } set { } }
        public string Description { get { return description; } set { } }
    }
}
