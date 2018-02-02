using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using LIB.Extensions;

namespace LIB.Data
{
    public class MUserEvent : Model<UserEvent>
    {
        public const string FIND_USEREVENTS_BY_SESSIONID = "[dbo].[GetUserEventsBySessionId]";

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

        //public UserEvents FindBySession(UserSession session)
        //{
        //    UserEvents list = new UserEvents();

        //    if (session.SessionId.Length.Equals(0))
        //    {
        //        throw new HttpException("sessionid.Length.Equals(0) on FindBySessionId");
        //    }

        //    ModelListLoader<UserEvent> loader = new ModelListLoader<UserEvent>();
        //    loader.Sql = MUserEvent.FIND_USEREVENTS_BY_SESSIONID;
        //    loader.AddLookupParameter("sessionid", SqlDbType.VarChar, 50, session.SessionId);
        //    loader.Model = ModelRegistry.Model<UserEvent>();
        //    loader.Attach(session.UserEvents);

        //    return list;
        //}
    }
}
