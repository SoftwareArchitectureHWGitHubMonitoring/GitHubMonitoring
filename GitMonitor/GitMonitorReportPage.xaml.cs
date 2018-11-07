using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for GitMonitorReportPage.xaml
    /// </summary>
    public partial class GitMonitorReportPage : Page
    {
        public GitMonitorReportPage()
        {
            InitializeComponent();
        }

        // Custom constructor to pass expense report data
        public GitMonitorReportPage(object data) : this()
        {
            // Bind to expense report data.
            this.DataContext = data;
        }
    }
}
