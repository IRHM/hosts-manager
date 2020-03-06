using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows;

namespace hosts_manager
{
    class ManageFile
    {
        public void update()
        {
            //MessageBox.Show("test");
        }

        public List<string[]> getCurrentHostRules()
        {
            // Instances of classes
            GetFile gf = new GetFile();

            // Get hosts file path
            string path = gf.Path();

            Contract.Requires(path != null);
            Contract.Requires(path.Length != 0);

            // Define rules list
            List<string[]> rules = new List<string[]>();

            using (StreamReader sr = new StreamReader(path))
            {
                foreach (string line in File.ReadLines(path))
                {
                    // Set line to currLine so its value can be modified
                    string currLine = line;

                    // Remove comment from line
                    int index = currLine.IndexOf("#");
                    if (index > -1)
                    {
                        currLine = currLine.Substring(0, index);
                    }

                    // If currLine is not empty it is a rule so add to dict
                    if (currLine != String.Empty)
                    {
                        // Seperate address and host in current rule/line
                        string[] ruleSeperated = currLine.Split(null);
                        string host = ruleSeperated[1];
                        string address = ruleSeperated[0];

                        // Add rule items to array
                        string[] rule = { host, address };

                        // Add individual rule to overall rules list
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }
    }
}
