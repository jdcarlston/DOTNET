using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using LIB.Extensions;

namespace LIB.Data
{
    public class MUserEvent : Model<UserEvent>
    {
        public const string FIND_USEREVENT_BY_ID = "[dbo].[GetUserEventById]";

        public MUserEvent() : base() { }

        internal override void LoadLine(DataRow row, UserEvent obj)
        {
            obj.Id = row.FieldValue("Id", obj.Id);
            obj.TS = row.FieldValue<DateTime>("TS", DateTime.MinValue);
            obj.Timestamp = row.FieldValue<DateTime>("Timestamp", DateTime.MinValue);
            obj.Description = row.FieldValue<string>("Description", obj.Description);

            //Cache the newly loaded object
            ModelObjectCache.CacheObject(obj);
        }

        public UserEvent FindById(string id)
        {
            UserEvent obj = new UserEvent();

            if (id.Length.Equals(0))
            {
                throw new HttpException("id.Length.Equals(0) on FindByTest");
            }

            using (SqlConnection cn = GetDefaultSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(FIND_USEREVENT_BY_ID, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = cmd.Parameters.Add("@id", SqlDbType.VarChar, 9);

                    prm.Value = id;
                    prm.Direction = ParameterDirection.Input;

                    //Execute and get dataset
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.IsTableFilled())
                    {
                        DoLoadLine(ds.Tables["Table"].Rows[0], obj);
                    }
                }
            }

            return obj;
        }
    }
}
