using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DOTNET
{
    /// <summary>
    /// Singleton that holds a reference to a Data.MapperRegistry object.
    /// </summary>
    public class DataSource
    {
        private static IDataSource _instance;

        public DataSource()
        { }

        public static void Init(IDataSource datasource)
        {
            _instance = datasource;
        }

        public static void Load(IModelObject obj)
        {
            _instance.Load(obj);
        }

        public static void Log(IModelObject obj)
        {
            _instance.Log(obj);
        }

        public static void Dispose(IModelObject obj)
        {
            _instance = null;
        }

        public interface IDataSource
        {
            void Load(IModelObject obj);
            void Log(IModelObject obj);
        }
    }
}
