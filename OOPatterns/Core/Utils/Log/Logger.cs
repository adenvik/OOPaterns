using System;
using System.IO;

namespace OOPatterns.Core.Utils.Log
{
    /// <summary>
    /// Logger to file
    /// </summary>
    class Logger : ILoggable
    {
        /// <summary>
        /// Path to log file
        /// </summary>
        public string PathToFile { get; set; } = "";

        /// <summary>
        /// Logged message to the file, if path not empty,else print to debug
        /// </summary>
        /// <param name="Message"></param>
        public void Log(string Message)
        {
            if (PathToFile != "")
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(PathToFile, FileMode.Append)))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{Message}");
            }
        }
    }
}
