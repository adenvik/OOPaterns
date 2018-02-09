using OOPatterns.Core.InternalObject.ParamObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.UserType
{
    public interface IUserType
    {
        void SetName(string name);
        string GetName();

        string GetImagePath();

        void AddParentObj(IUserType parent);
        void RemoveParentObj(IUserType parent);
        List<IUserType> GetParents();

        void AddVariable(Variable variable);
        void RemoveVariable(Variable variable);
        List<IParamObject> GetVariables();

        void AddMethod(Method method);
        void RemoveMethod(Method method);
        IParamObject GetMethod(int index);
        List<IParamObject> GetMethods();

        bool Equals(object obj);
    }
}
