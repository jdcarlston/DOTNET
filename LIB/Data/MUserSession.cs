using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using LIB.Extensions;

namespace LIB.Data
{
    public class MUserSession : Model<UserSession>
    {
        public const string FIND_USERSESSION_BY_SESSIONID = "[dbo].[GetUserSessionBySessionId]";
        
        public MUserSession() : base() { }

        internal override void LoadLine(DataRow row, UserSession obj)
        {
            obj.Id = row.FieldValue("Id", obj.Id);
            obj.TS = row.FieldValue<DateTime>("TS", obj.TS);
            obj.Agent = row.FieldValue<string>("Agent", obj.Agent);
            obj.Browser = row.FieldValue<string>("Browser", obj.Browser);
            obj.SessionId = row.FieldValue<string>("SessionId", obj.SessionId);

            loadUserEvents(obj);

            //Cache the newly loaded object
            ModelObjectCache.CacheObject(obj);
        }

        public UserSession FindBySessionId(string sessionid)
        {
            UserSession obj = new UserSession();

            if (sessionid.Length.Equals(0))
            {
                throw new HttpException("sessionid.Length.Equals(0) on FindBySessionId");
            }

            using (SqlConnection cn = GetDefaultSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(FIND_USERSESSION_BY_SESSIONID, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = cmd.Parameters.Add("@sessionid", SqlDbType.VarChar, 9);

                    prm.Value = sessionid;
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

        private void loadUserEvents(UserSession session)
        {
            ModelListLoader<UserEvent> loader = new ModelListLoader<UserEvent>();
            loader.Sql = MUserEvent.FIND_USEREVENTS_BY_SESSIONID;
            loader.AddLookupParameter("sessionid", SqlDbType.VarChar, 100, session.SessionId);
            loader.Model = ModelRegistry.Model<UserEvent>();
            loader.Attach(session.UserEvents);
        }
    }
}
