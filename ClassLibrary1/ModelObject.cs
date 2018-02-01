using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB
{
    /// <summary>
    /// Base class for all id based domain objects.
    /// </summary>
    [Serializable]
    public class ModelObject : Ghostable, IModelObject
    {
        protected int _id;
        protected DateTime _ts = DateTime.Now;

        public ModelObject()
        {
            //MarkLoaded();
        }

        public ModelObject(int id)
        {
            this.Id = id;
        }

        protected void Load()
        {
            if (IsGhost && Id > 0)
            {
                DataSource.Load(this);
            }
        }

        protected void Log()
        {
            if (IsLoaded && Id > 0)
                DataSource.Log(this);
        }

        [XmlAttribute]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlAttribute]
        public DateTime TS
        {
            get { return _ts; }
            set { _ts = value; }
        }

        public XmlAttributeOverrides GetListRootOverride()
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attr = new XmlAttributes();
            attr.XmlRoot = new XmlRootAttribute(this.GetType().Name + "s");
            overrides.Add(typeof(List<IModelObject>), attr);

            return overrides;
        }

        public bool Equals(ModelObject obj)
        {
            if (obj == null) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            if (obj.IsLoaded
                && _id.Equals(obj.Id)) return true;

            return false;
        }
    }
}
