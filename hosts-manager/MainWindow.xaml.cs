using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        // Instances of classes
        static ManageFile mf = new ManageFile();
        static ErrorHandler eh = new ErrorHandler();

        public MainWindow()
        {
            InitializeComponent();

            AddCurrentHostRules();
        }

        private void AddCurrentHostRules()
        {
            // Clear current items from listbox
            hostsListBox.Items.Clear();

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

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get textBox data
            string host = HostTextBox.Text;
            string address = AddressTextBox.Text;

            ObservableCollection<Host> hostData = new ObservableCollection<Host>();

            // Add data to Host structure
            hostData.Add(new Host()
            {
                Name = host,
                Address = address
            });

            // Save host to hosts file
            mf.addHostRule($"{address} {host}");

            // Add new host to listbox
            hostsListBox.Items.Add(hostData);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private List<TextBlock> GetHostsCheckedTextBlocks()
        {
            List<TextBlock> checkedTextBlocks = new List<TextBlock>();
            int i = 0;

            foreach (StackPanel sp in FindVisualChildren<StackPanel>(hostsListBox))
            {
                foreach (CheckBox cb in FindVisualChildren<CheckBox>(sp))
                {
                    // If checkbox isn't checked skip iteration
                    if (cb.IsChecked == false)
                    {
                        continue;
                    }

                    // Add textblock to list if checkbox is ticked
                    foreach (TextBlock tb in FindVisualChildren<TextBlock>(sp))
                    {
                        checkedTextBlocks.Add(tb);
                    }
                }

                i++;
            }

            return checkedTextBlocks;
        }

        private void HostCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            disableBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
        }

        private void HostCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in FindVisualChildren<CheckBox>(hostsListBox))
            {
                // If one checkbox is checked return to stop function
                if (cb.IsChecked == true)
                {
                    return;
                }
            }

            // If function isn't stopped above then..
            // ..all checkboxes are unticked so enable buttons
            disableBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get all checked items textblock values and delete from hosts file
            foreach (TextBlock tb in GetHostsCheckedTextBlocks())
            {
                string rule = tb.Text;
                mf.deleteHostRule(rule);
            }

            // After deleting refresh listbox
            AddCurrentHostRules();
        }

        private void CopyCtx_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get listboxitem that copy was clicked on
                ContextMenu cm = (ContextMenu)ContextMenu.ItemsControlFromItemContainer((MenuItem)e.OriginalSource);
                UIElement lbi = cm.PlacementTarget; // <- listboxitem

                // Get textblock in listboxitem
                foreach (TextBlock tb in FindVisualChildren<TextBlock>(lbi))
                {
                    string rule = tb.Text;

                    // Copy rule to clipboard
                    Clipboard.SetText(rule);
                }
            }
            catch (Exception ex)
            {
                eh.ShowError(ex);
            }
        }
    }
}
