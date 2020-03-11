using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace hosts_manager
{
    class ManageFile
    {
        // Get hosts file path
        static GetFile gf = new GetFile();
        string hostsFilePath = gf.Path();

        public List<string[]> getCurrentHostRules()
        {
            Contract.Requires(hostsFilePath != null);
            Contract.Requires(hostsFilePath.Length != 0);

            // Define rules list
            List<string[]> rules = new List<string[]>();

            using (StreamReader sr = new StreamReader(hostsFilePath))
            {
                foreach (string line in File.ReadLines(hostsFilePath))
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

        public void deleteHostRule(string rule)
        {
            var name = rule.Split(' ')[0].ToString();
            var address = rule.Split(' ')[1].ToString();
            var ruleToDelete = $"{address} {name}";

            // Load all lines into list
            List<String> lines = File.ReadAllLines(hostsFilePath).ToList();

            // Write all lines back into hostsfile, unless it it the one to delete
            using (StreamWriter writer = new StreamWriter(hostsFilePath))
            {
                foreach (var line in lines)
                {
                    if (String.Compare(line, ruleToDelete) == 0)
                    {
                        continue;
                    }

                    writer.WriteLine(line);
                }
            }
        }
    }
}
