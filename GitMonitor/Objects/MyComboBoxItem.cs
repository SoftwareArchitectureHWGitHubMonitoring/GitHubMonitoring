using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class MyComboBoxItem
    {
        private string name; 
        public MyComboBoxItem(string name)
        {
            this.name = name;
        }
        public string Name { get { return name; } set { } }
    }
}
