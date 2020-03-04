using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace hosts_manager
{
    public class Host
    {
        public string Name
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
 
            //UpdateFile uf = new UpdateFile();
            //uf.test();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get textBox data
            string host = hostTextBox.Text;
            string address = addressTextBox.Text;

            ObservableCollection<Host> hostData = new ObservableCollection<Host>();

            // Add data to Host structure
            hostData.Add(new Host()
            {
                Name = host,
                Address = address
            });

            // Add new host to listbox
            hostsListBox.Items.Add(hostData);
        }
    }
}
