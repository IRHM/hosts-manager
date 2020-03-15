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
        // Class references
        static GetFile gf = new GetFile();
        static ErrorHandler eh = new ErrorHandler();

        // Get hosts file path
        string hostsFilePath = gf.Path();

        public List<string[]> getCurrentHostRules()
        {
            Contract.Requires(hostsFilePath != null);
            Contract.Requires(hostsFilePath.Length != 0);

            // Define rules list
            List<string[]> rules = new List<string[]>();

            string[] ruleSeperated = null;

            try
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
                        ruleSeperated = currLine.Split(null);
                        string host = ruleSeperated[1];
                        string address = ruleSeperated[0];

                        // Add rule items to array
                        string[] rule = { host, address };

                        // Add individual rule to overall rules list
                        rules.Add(rule);
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                var brokenRule = ruleSeperated[0];

                eh.ShowError(ex, $"Try removing rule '{brokenRule}' from hosts file.");
            }
            catch (Exception ex)
            {
                eh.ShowError(ex);
            }

            return rules;
        }

        public void addHostRule(string rule)
        {
            StreamWriter sw = null;

            try
            {
                // (create &) open file to write rule
                using (sw = File.AppendText(hostsFilePath))
                {
                    // TODO: If line is empty, just write to that line instead of going to newline
                    sw.Write($"\n{rule}");
                }
            }
            catch (IOException ex)
            {
                eh.ShowError(ex);
            }
            catch (Exception ex)
            {
                eh.ShowError(ex);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public void deleteHostRule(string rule)
        {
            var name = rule.Split(' ')[0].ToString();
            var address = rule.Split(' ')[1].ToString();
            var ruleToDelete = $"{address} {name}";

            // Load all lines into list
            List<String> lines = File.ReadAllLines(hostsFilePath).ToList();

            StreamWriter sw = null;

            try
            {
                // Write all lines back into hostsfile, unless it it the one to delete
                using (sw = new StreamWriter(hostsFilePath))
                {
                    foreach (var line in lines)
                    {
                        if (String.Compare(line, ruleToDelete) == 0)
                        {
                            continue;
                        }

                        sw.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                eh.ShowError(ex);
            }
            catch (Exception ex)
            {
                eh.ShowError(ex);
            }
            finally
            {
                if(sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
