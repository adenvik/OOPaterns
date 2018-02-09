using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Modificators;
using OOPatterns.Core.Utils.Type;
using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject
{
    public class Core
    {
        public const string CLASS = "class";
        public const string INTERFACE = "interface";

        List<IUserType> objects;
        List<IVisualObject> visualObjects;

        private static Core core = null;
        private static int currentLanguage = -1;

        public const int CSHARP = 0;
        public const int JAVA = 1;

        public Utils.Type.Type Type { get; }
        public Access Access { get; }

        private Core(int language)
        {
            objects = new List<IUserType>();
            visualObjects = new List<IVisualObject>();

            switch (language)
            {
                case 1:
                    //TO DO
                    Type = new Utils.Type.Type(TypeInitializer.Language.Java);
                    currentLanguage = JAVA;
                    break;
                case 0:
                default:
                    currentLanguage = CSHARP;
                    Type = new Utils.Type.Type();
                    break;
            }
            Access = new Access();
        }

        public List<IUserType> GetObjects()
        {
            return objects;
        }

        public List<IVisualObject> GetVisualObjects()
        {
            return visualObjects;
        }

        public void AddObject(IUserType obj)
        {
            objects.Add(obj);
            visualObjects.Add(new VisualObject(obj));
        }

        public Tuple<IUserType, IVisualObject> GetObjectByName(string name)
        {
            var vObj = visualObjects.Find(obj => (obj as VisualObject).OBJECT_NAME == name);
            return new Tuple<IUserType, IVisualObject>(vObj?.GetUserType(), vObj);
        }

        public Tuple<IUserType, IVisualObject> Last()
        {
            if (objects.Count == 0) return new Tuple<IUserType, IVisualObject>(null, null);
            return new Tuple<IUserType, IVisualObject>(objects[objects.Count - 1], visualObjects[objects.Count - 1]);
        }

        public void Remove(IUserType userType)
        {
            int index = objects.FindIndex(obj => obj.Equals(userType));
            objects.RemoveAt(index);
            visualObjects.RemoveAt(index);
        }

        public IVisualObject GetVisualObject(IUserType userType)
        {
            return visualObjects[objects.FindIndex(obj => obj.Equals(userType))];
        }

        public static Core GetInstance(int language = -1)
        {
            if (core == null)
            {
                core = new Core(language);
                return core;
            }
            if (language != currentLanguage && language != -1)
            {
                //TODO: изменение типов, если это необходимо
            }
            return core;
        }
    }
}
