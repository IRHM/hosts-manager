using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

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

            addCurrentHostRules();
        }

        private void addCurrentHostRules()
        {
            // Instances of classes
            ManageFile mf = new ManageFile();

            // Loop over hosts in array returned by `mf.getCurrentHostRules()`..
            // ..And add them to listbox
            foreach (var rule in mf.getCurrentHostRules())
            {
                string host = rule[0];
                string address = rule[1];

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

            // Save host to JSON file
            TextWriter writer = null;
            try
            {
                // Set path to hosts file
                GetFile gf = new GetFile();
                var savePath = gf.Path();

                // (create &) open file to write rule
                using (StreamWriter sw = File.AppendText(savePath))
                {
                    sw.Write($"{address} {host}\n");
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            // Add new host to listbox
            hostsListBox.Items.Add(hostData);
        }
    }
}
