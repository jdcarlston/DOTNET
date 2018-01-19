using DOTNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET.Data
{
    /// <summary>
    /// Lazy load mapper stub for a lists
    /// </summary>
    public class ModelListLoader<T> : IDisposable where T : ModelObject, IModelObject, IDisposable, new()
    {
        public string Sql = String.Empty;

        public bool IsProcedure = true;
        private SqlCommand _cmd = new SqlCommand();

        private Model _model;

        public Model Model
        {
            get
            {
                if (_model == null)
                {
                    _model = ModelRegistry.Model<T>();
                }

                return _model;
            }
            set { _model = value; }
        }

        public SqlCommand Cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        public void FindStatement(string sproc)
        {
            FindStatement(sproc, AppCache.DefaultDatabase);
        }
        public void FindStatement(string sproc, string database)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(database);
            sb.Append(".");
            sb.Append(Model.DefaultSchema);
            sb.Append(".");
            sb.Append(sproc.Replace("Object", typeof(T).Name));

            Sql = sb.ToString();
        }

        public void AddLookupParameter(string name)
        {
            AddLookupParameter(name, SqlDbType.Int, DBNull.Value);
        }

        public void AddLookupParameter(string name, int paramValue)
        {
            try
            {
                AddLookupParameter(name, SqlDbType.Int, Convert.ToInt32(paramValue));
            }
            catch
            {
                AddLookupParameter(name, SqlDbType.BigInt, paramValue);
            }
        }

        public void AddLookupParameter(string name, SqlDbType type, object paramValue)
        {
            SqlParameter prm = Cmd.Parameters.Add("@" + name, type);
            prm.Value = paramValue;
        }

        public void AddLookupParameter(string name, SqlDbType type, int size, object paramValue)
        {
            SqlParameter prm = Cmd.Parameters.Add("@" + name, type, size);
            prm.Value = paramValue;
        }

        public void Attach(ModelList<T> list)
        {
            list.RunLoader = new ModelList<T>.Loader(Load);
        }

        public void Attach<U>(ModelList<T> list, U obj) where U : IModelObject, new()
        {
            Type FKtype = typeof(U);

            Attach(list, FKtype.Name, "id", obj.Id);
        }
        public void Attach(ModelList<T> list, string keytype, int id)
        {
            Attach(list, keytype, "id", id);
        }
        public void Attach(ModelList<T> list, string keytype, string paramname, int id)
        {
            //Type Ttype = typeof(T);

            ModelListLoader<T> loader = new ModelListLoader<T>();
            loader.FindStatement(Model.GET_LIST_BY_FORIEGN_KEY.Replace("List", list.GetType().Name).Replace("ForeignKey", keytype));

            try
            {
                loader.AddLookupParameter(paramname, SqlDbType.Int, Convert.ToInt32(id));
            }
            catch
            {
                loader.AddLookupParameter(paramname, SqlDbType.BigInt, id);
            }

            loader.Model = ModelRegistry.Model<T>();
            loader.Attach(list);
        }

        public void Attach<U>(ModelList<T> list, ModelList<U> obj) where U : IModelObject, new()
        {
            Attach<U>(list, obj.GetIds());
        }
        public void Attach<U>(ModelList<T> list, List<int> idlist) where U : IModelObject, new()
        {
            Type FKtype = typeof(U);

            string[] strlist = Array.ConvertAll<int, string>(idlist.ToArray(), new Converter<int, string>(Convert.ToString));

            Attach(list, FKtype.Name, String.Join(",", strlist));
        }
        public void Attach<U>(ModelList<T> list, List<string> keylist) where U : IModelObject, new()
        {
            Type FKtype = typeof(U);

            Attach(list, FKtype.Name, keylist.ToQuoteString());
        }
        public void Attach(ModelList<T> list, string keytype, string keylist)
        {
            //Type Ttype = typeof(T);

            ModelListLoader<T> loader = new ModelListLoader<T>();
            loader.FindStatement(Model.GET_LIST.Replace("List", list.GetType().Name));
            loader.AddLookupParameter("keytype", SqlDbType.VarChar, 50, keytype);
            loader.AddLookupParameter("keylist", SqlDbType.VarChar, -1, keylist);

            loader.Model = ModelRegistry.Model<T>();
            loader.Attach(list);
        }

        public void Associate<U>(ModelList<T> list, T obj, U uobj) where U : IModelObject, new()
        {
            Type FKtype = typeof(U);

            Associate(list, obj, FKtype.Name, uobj.Id);
        }

        public void Associate(ModelList<T> list, T obj, string parenttype, int parentid)
        {
            ModelListLoader<T> loader = new ModelListLoader<T>();
            loader.FindStatement(Model.ASSOCIATE.Replace("Parent", parenttype));
            loader.AddLookupParameter("childid", obj.Id);
            loader.AddLookupParameter("parentid", parentid);

            loader.Model = ModelRegistry.Model<T>();
            loader.Attach(list, obj);
        }

        public void Load(ModelList<T> list)
        {
            using (SqlConnection cn = GetSqlConnection())
            {
                //Cmd.CommandTimeout = 5000;
                Cmd.CommandText = Sql;
                Cmd.Connection = cn;
                Cmd.CommandTimeout = 0;

                cn.Open();

                if (IsProcedure)
                    Cmd.CommandType = CommandType.StoredProcedure;
                else
                    Cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = Cmd;

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    //T obj = GhostForLine(row);
                    T obj = new T();
                    
                    obj.MarkLoading();
                    Model.DoLoadLine(row, obj);
                    obj.MarkLoaded();

                    list.Add(obj);
                }
            }
        }

        private T GhostForLine(DataRow row)
        {
            T obj = ((Model<T>)Model).Find((int)row[Model.KeyColumnName]);

            return obj;
        }

        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(Model.ConnectString);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                Cmd = null;
                Model = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}
