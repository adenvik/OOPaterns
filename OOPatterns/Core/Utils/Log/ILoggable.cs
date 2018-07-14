using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Log
{
    public interface ILoggable
    {
        string PathToFile { get; set; }
        void Log(string Message);
    }
}
