using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Log
{
    interface ILoggable
    {
        void SetPathToFile(string Path);
        void Log(string Message);
    }
}
