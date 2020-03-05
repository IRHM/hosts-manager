using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Windows;

namespace hosts_manager
{
    class ManageFile
    {
        public void update()
        {
            //MessageBox.Show("test");
        }

        public Array getCurrentHostRules()
        {
            // Instances of classes
            GetFile gf = new GetFile();

            // Get hosts file path
            string path = gf.Path();

            Contract.Requires(path != null);
            Contract.Requires(path.Length != 0);

            List<String> rules = new List<String>();

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

                    if (currLine != String.Empty)
                    {
                        // Add all rules to array
                        rules.Add(currLine);
                    }
                }
            }

            return rules.ToArray();
        }
    }
}
