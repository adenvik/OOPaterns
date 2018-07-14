using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Log
{
    class Logger : ILoggable
    {
        public string PathToFile { get; set; } = "";

        public void Log(string Message)
        {
            if (PathToFile != "")
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(PathToFile, FileMode.Append)))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{Message}");
                }
            }
            System.Diagnostics.Debug.WriteLine(Message);
        }

        public void SetPathToFile(string Path)
        {
            PathToFile = Path;
        }
    }
}
