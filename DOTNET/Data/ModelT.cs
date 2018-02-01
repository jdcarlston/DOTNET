using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LIB;

namespace LIB.Data
{
    public abstract class Model<T> : Model where T : IModelObject, new()
    {
        internal abstract void LoadLine(DataRow row, T obj);

        public Model() : base() { }
        //public Mapper(string cs) : base(cs) { }
        //public Mapper(string cs, string schema) : base(cs, schema) { }
        //public Mapper(string schema) : base(schema) { }

        public T CreateGhost(int id)
        {
            T obj = new T();
            obj.Id = id;
            
            //See if the object is cached, if so return it.
            //if (ModelObjectCache.IsObjectCached<T>(id))
            //{
            //    obj.MarkLoading();
            //    ModelObjectCache.GetCachedObject<T>(obj);
            //    obj.MarkLoaded();
            //}
            //else
            //{
            //    obj.MarkGhost();
            //}

            obj.MarkGhost();

            return obj;
        }

        public override void Load(IModelObject obj)
        {
            //Get From Database
            if (ModelObjectCache.IsObjectCached<T>(obj))
            {
                obj.MarkLoading();
                ModelObjectCache.GetCachedObject<T>(obj);
                obj.MarkLoaded();
            }
            else if (obj.Id > 0)
            {
                obj.MarkLoading();
                GetFromDatabase(obj);
                obj.MarkLoaded();
            }
        }

        public override void Log(IModelObject obj)
        {
            LogInDatabase(obj);
        }

        private void LogInDatabase(IModelObject obj)
        {
            using (SqlConnection conn = GetDefaultSqlConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(BuildStatement((obj.Id > 0) ? Statements.UPDATE : Statements.ADD), conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = new SqlParameter();
                    prm = cmd.Parameters.Add("doc", SqlDbType.VarChar, -1);
                    prm.Value = ((T)obj).Serialize();
                    prm.Direction = ParameterDirection.Input;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                        DoLoadLine(ds.Tables[0].Rows[0], obj);
                }
            }
        }
        private void GetFromDatabase(IModelObject obj)
        {
            using (SqlConnection conn = GetDefaultSqlConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(BuildStatement(Statements.FIND), conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (obj.Id > 0)
                    {
                        SqlParameter prm = cmd.Parameters.Add("id", SqlDbType.Int);
                        prm.Value = obj.Id;
                        prm.Direction = ParameterDirection.Input;
                    }

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.IsTableFilled())
                        DoLoadLine(ds.Tables["Table"].Rows[0], obj);
                }
            }
        }

        public T Find(int id)
        {
            return CreateGhost(id);
        }

        protected override string BuildStatement(Statements type)
        {
            string sproc = string.Empty;
            switch (type)
            {
                case Statements.ADD: sproc = ADD; break;
                case Statements.UPDATE: sproc = UPDATE; break;
                default: sproc = GET_BY_UNIQUE_KEY; break;
            }

            return BuildStatement(sproc.Replace("Object", typeof(T).Name));
        }

        protected virtual string BuildStatement(string sproc)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(AppCache.DefaultDatabase);
            sb.Append(".");
            sb.Append(DefaultSchema);
            sb.Append(".");
            sb.Append(sproc);

            return sb.ToString();
        }

        internal override void DoLoadLine(DataRow row, IModelObject obj)
        {
            //Get From Database
            if (!ModelObjectCache.IsObjectCached<T>(obj))
            {
                LoadLine(row, (T)obj);
            }
            else
            {
                //Check in Cache
                ModelObjectCache.GetCachedObject<T>(obj);
            }
        }

        internal override string KeyColumnName
        {
            get { return "Id"; }
        }
    }
}
