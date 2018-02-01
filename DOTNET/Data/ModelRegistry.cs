using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DOTNET;

namespace DOTNET.Data
{
    /// <summary>
    /// Singleton for storing references to specific object mapper objects.
    /// </summary>
    /// <summary>
    /// Singleton for storing references to specific object mapper objects.
    /// </summary>
    public class ModelRegistry : DOTNET.DataSource.IDataSource
    {
        private static ModelRegistry _instance = new ModelRegistry();
        private IDictionary _models = new Hashtable();

        public ModelRegistry()
        {
            DOTNET.DataSource.Init(this);
        }

        public void Load(IModelObject obj)
        {
            Model(obj.GetType()).Load(obj);
        }

        public void Log(IModelObject obj)
        {
            Model(obj.GetType()).Log(obj);
        }

        public static Model Model(Type type)
        {
            Model m = (Model)_instance._models[type];

            //if (m == null)
            //{
                m = LoadModel(type);
                _instance._models[type] = m;
            //}

            return m;
        }

        public static Model<T> Model<T>() where T : IModelObject, new()
        {
            return (Model<T>)Model(typeof(T));
        }

        private static Model LoadModel(Type type)
        {
            Model m = null;

            switch (type.Name)
            {
                //Need one case block per domain object
                case "Test": m = new MTest(); break;
                case "UserEvent": m = new MUserEvent(); break;
            }

            return m;
        }

        private static Model<T> LoadMapper<T>() where T : IModelObject, new()
        {
            return Model<T>();
        }
    }
}
