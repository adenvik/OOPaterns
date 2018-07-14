using OOPatterns.Core.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core
{
    public class Core
    {
        public const string CLASS = "class";
        public const string INTERFACE = "interface";
        public ILoggable Logger { get; }

        public List<Element> Objects { get; set; } = new List<Element>();

        private static Core core = null;

        private Core()
        {
            Logger = new Logger();
        }

        public static Core GetInstance()
        {
            return core ?? (core = new Core());
        }
    }
}
