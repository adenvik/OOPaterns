using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Helpers
{
    public class Unique
    {
        public int Id { get; set; }
        private static int id = 0;

        public void GenerateId()
        {
            Id = id;
            id++;
        }
    }
}
