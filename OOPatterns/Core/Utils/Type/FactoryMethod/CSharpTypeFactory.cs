using OOPatterns.Core.Utils.Type.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type.FactoryMethod
{
    class CSharpTypeFactory : TypeFactory
    {
        protected override IMethodType MethodTypeCreate()
        {
            return new CSharpMethodType();
        }

        protected override IType TypeCreate()
        {
            return new CSharpType();
        }

        protected override IVariableType VariableTypeCreate()
        {
            return new CSharpVariableType();
        }
    }
}
