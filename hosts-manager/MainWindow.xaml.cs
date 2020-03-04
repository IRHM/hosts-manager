using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
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

            // Save host to JSON file
            TextWriter writer = null;
            try
            {
                // Serialize hostData with formatting
                var hostJSON = JsonConvert.SerializeObject(hostData, Formatting.Indented);

                // Set save file to hosts.json inside folder that the .exe is running in
                var savePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/hosts.json";

                // (create &) open file to write JSON
                using(StreamWriter sw = File.AppendText(savePath))
                {
                    sw.Write($"{hostJSON}\n");
                }
            }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }

            // Add new host to listbox
            hostsListBox.Items.Add(hostData);
        }
    }
}
