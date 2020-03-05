using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hosts_manager
{
    class GetFile
    {
        public string Path()
        {
            return GetPath();
        }

        public string GetPath()
        {
            string path = "C:\\Windows\\System32\\drivers\\etc\\hosts";
            bool exists = File.Exists(path);

            if (exists)
            {
                // If hosts already exists just return its path
                return path;
            }
            else
            {
                // If hosts doesn't exists create it and return its path
                File.Create(path).Dispose();
                return path;
            }
        }
    }
}
