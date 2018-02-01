using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DOTNET;

namespace DOTNET.Data
{
    /// <summary>
    /// Base class for all domain object model mappers.
    /// Search:
    /// public (\w+) (\w+) { get { Load\(\); return _\w+; } set { Load\(\); _\w+ = value; } }
    /// Replace:
    /// certificate.\2 = row.FieldValue<\1>("\2", certificate.\2);
    /// </summary>
    public abstract class Model
    {
        public enum Statements { FIND, ADD, UPDATE, ASSOCIATE };

        public string GET_BY_UNIQUE_KEY = "[GetObjectById]";
        public string GET_LIST_BY_FORIEGN_KEY = "[GetListByForeignKeyId]";
        public string ADD = "[AddObject]";
        public string UPDATE = "[UpdateObject]";
        public string ASSOCIATE = "[AssociateObjectWithParent]";  //Many to Many

        public string GET_LIST = "[GetList]";

        public abstract void Load(IModelObject obj);
        public abstract void Log(IModelObject obj);

        internal abstract void DoLoadLine(DataRow row, IModelObject obj);
        protected abstract string BuildStatement(Statements type);
        internal abstract string KeyColumnName { get; }

        private string _connectString = AppCache.DbConnStr;
        private string _defaultschema = "[dbo]";

        public Model() { }
        
        public string ConnectString
        {
            get { return _connectString; }
            set { _connectString = value; }
        }

        public string DefaultSchema
        {
            get { return _defaultschema; }
            set { _defaultschema = value; }
        }

        protected SqlConnection GetDefaultSqlConnection()
        {
            return new SqlConnection(_connectString);
        }

        public void LoadLine(DataRow row, IModelObject obj)
        {
            DoLoadLine(row, obj);
        }

        internal DataSet GetDataSetFromXmlQuery(string sproc, XmlDocument prmvalue)
        {
            using (SqlConnection cn = GetDefaultSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sproc, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 360;

                    SqlParameter prm = new SqlParameter();

                    prm = cmd.Parameters.Add("@xml", SqlDbType.VarChar, -1);
                    prm.Value = prmvalue.InnerXml;
                    prm.Direction = ParameterDirection.Input;

                    //Execute and get dataset
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.SelectCommand.CommandTimeout = 360;

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    return ds;                    
                }
            }
        }

    }
}
