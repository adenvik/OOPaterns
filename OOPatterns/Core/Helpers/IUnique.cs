using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Helpers
{
    public interface IUnique
    {
        int Id { set; get; }
        void GenerateId();
    }
}
