using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Helpers
{
    /// <summary>
    /// Unique identifier of object
    /// </summary>
    public class Unique
    {
        /// <summary>
        /// Id of unique object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Static value, describing count of an unique objects
        /// </summary>
        private static int id = 0;

        public Unique()
        {
            Id = id;
            id++;
        }
    }
}
