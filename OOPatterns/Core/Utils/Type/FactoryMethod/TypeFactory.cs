using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type.FactoryMethod
{
    abstract class TypeFactory
    {
        public IType Type { get; private set; }
        public IMethodType MethodType { get; private set; }
        public IVariableType VariableType { get; private set; }

        public void Generate()
        {
            Type = TypeCreate();
            MethodType = MethodTypeCreate();
            VariableType = VariableTypeCreate();
        }

        protected abstract IType TypeCreate();
        protected abstract IMethodType MethodTypeCreate();
        protected abstract IVariableType VariableTypeCreate();
    }
}
