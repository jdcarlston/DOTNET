using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOTNET;

namespace DOTNET.Data
{
    /// <summary>
    /// Singleton class to support caching of DomainObjects based on their id value.
    ///	 A two level Hashtable stucture is used to support the cache database.
    ///	 The first level contains one entry per ModelObject derived type with the
    ///	 type as the key. The second level contains one entry per object with the id
    ///	 as the key.
    /// </summary>
    /// 
    public class ModelObjectCache
    {
        private static ModelObjectCache _instance = new ModelObjectCache();
        private Hashtable _mobjects = new Hashtable();

        public ModelObjectCache()
        { }

        public static void CacheObject<T>(T obj) where T : IModelObject
        {
            Type type = typeof(T);
            if (!_instance._mobjects.ContainsKey(type)) _instance._mobjects[type] = new Hashtable();
            Hashtable typecache = (Hashtable)_instance._mobjects[type];

            typecache[obj.Id] = obj;
        }

        public static void ClearCache<T>() where T : IModelObject
        {
            Type type = typeof(T);
            if (!_instance._mobjects.ContainsKey(type))
            {
                _instance._mobjects[type] = new Hashtable();
            }
            else
            {
                Hashtable typecache = (Hashtable)_instance._mobjects[type];
                typecache.Clear();
            }
        }
        public static void ClearObject<T>(int id) where T : IModelObject
        {
            Type type = typeof(T);
            if (!_instance._mobjects.ContainsKey(type))
            {
                _instance._mobjects[type] = new Hashtable();
            }
            else
            {
                Hashtable typecache = (Hashtable)_instance._mobjects[type];
                typecache.Remove(id);
            }
        }

        public static List<T> GetCachedObjectsOfType<T>() where T : IModelObject
        {
            Type type = typeof(T);
            if (!_instance._mobjects.ContainsKey(type)) _instance._mobjects[type] = new Hashtable();
            Hashtable typecache = (Hashtable)_instance._mobjects[type];
            return typecache.Values.OfType<T>().ToList();
            //return _instance._mobjects.OfType<T>().ToList();
        }

        public static T GetCachedObject<T>(object key) where T : IModelObject
        {
            Type type = typeof(T);
            Hashtable typecache = (Hashtable)_instance._mobjects[type];
            if (typecache != null)
                return (T)typecache[key];
            else
                return default(T);
        }

        public static bool IsObjectCached<T>(object key) where T : IModelObject
        {
            Type type = typeof(T);
            Hashtable typecache = (Hashtable)_instance._mobjects[type];
            if (typecache != null)
                return typecache.ContainsKey(key);
            else
                return false;
        }
    }
}
