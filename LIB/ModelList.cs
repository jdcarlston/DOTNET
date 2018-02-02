using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LIB
{
    /// <summary>
    /// ModelList is a ghostable list of domain objects
    /// </summary>
    public abstract class ModelList<T> : Ghostable, IEnumerable, IModelList where T : IModelObject, new()
    {
        private List<T> _data = new List<T>();

        public delegate void Loader(ModelList<T> list);
        public Loader RunLoader;

        public List<T> Items
        {
            get { return _data; }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public void Add(object obj)
        {
            Add((T)obj);
        }

        public void Remove(object obj)
        {
            Remove((T)obj);
        }

        public void Add(T obj)
        {
            Items.Add(obj);
        }
        public void AddRange(ModelList<T> list)
        {
            list.ForEach(x => Items.Add(x));
        }

        public void Remove(T obj)
        {
            Items.Remove(obj);
        }

        public void Load()
        {
            if (IsGhost && RunLoader != null)
            {
                MarkLoading();
                RunLoader(this);
                MarkLoaded();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return (Items as IEnumerable<T>).GetEnumerator();
        }


        public bool Exists(Predicate<T> match)
        {
            return Items.Exists(match);
        }

        public T FindById(int id)
        {
            foreach (T obj in Items)
                if (obj.Id.Equals(id)) return obj;
            return default(T);
        }

        public T First()
        {
            if (Items.Count > 0)
                return Items.First();
            else
                return new T();
        }

        public T Last()
        {
            if (Items.Count > 0)
                return Items.Last();
            else
                return new T();
        }

        public T Find(Predicate<T> match)
        {
            return Items.Find(match);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            return Items.FindAll(match);
        }

        public void TrueForAll(Predicate<T> match)
        {
            Items.TrueForAll(match);
        }

        public void ForEach(Action<T> action)
        {
            Items.ForEach(action);
        }

        public void ForEach(Comparison<T> comparison)
        {
            Items.Sort(comparison);
        }

        public List<int> GetIds()
        {
            List<int> ids = new List<int>();
            foreach (T i in Items)
            {
                ids.Add(i.Id);
            }

            return ids;
        }
    }
}
