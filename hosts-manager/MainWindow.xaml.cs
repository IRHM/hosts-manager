﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        static GetFile gf = new GetFile();
        static ManageFile mf = new ManageFile();

        // hosts file path
        string hostsFilePath = gf.Path();

        public MainWindow()
        {
            InitializeComponent();

            addCurrentHostRules();
        }

        private void addCurrentHostRules()
        {
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

            // Save host to hosts file
            TextWriter writer = null;
            try
            {
                // (create &) open file to write rule
                using (StreamWriter sw = File.AppendText(hostsFilePath))
                {
                    sw.Write($"\n{address} {host}");
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

        public static IEnumerable<T> findVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
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

                    foreach (T childOfChild in findVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void hostCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            copyBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
        }

        private void hostCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in findVisualChildren<CheckBox>(hostsListBox))
            {
                // If one checkbox is checked return to stop function
                if (cb.IsChecked == true)
                {
                    return;
                }
            }

            // If function isn't stopped above then..
            // ..all checkboxes are unticked so enable buttons
            copyBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;

            foreach (StackPanel sp in findVisualChildren<StackPanel>(hostsListBox))
            {
                foreach (CheckBox cb in findVisualChildren<CheckBox>(sp))
                {
                    // If checkbox isn't checked break from loop
                    if (cb.IsChecked == false)
                    {
                        //MessageBox.Show($"unchecked:  - {cb.IsChecked}");
                        break;
                    }

                    // TODO: Remove Item after deleting rule from hosts file

                    // Remove item
                    Object item = hostsListBox.Items[i];
                    hostsListBox.Items.RemoveAt(hostsListBox.Items.IndexOf(item));

                    // Delete rule from hosts file if checked
                    foreach (TextBlock tb in findVisualChildren<TextBlock>(sp))
                    {
                        string rule = tb.Text;

                        //mf.deleteHostRule(rule);
                    }
                }

                i++;
            }
        }
    }
}
