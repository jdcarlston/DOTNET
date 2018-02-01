using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Data
{
    public class MUserEvent : Model<UserEvent>
    {
        public const string FIND_UserEvent_BY_PLAN = "[dbo].[GetUserEventByTestId]";

        public MUserEvent() : base() { }

        internal override void LoadLine(DataRow row, UserEvent obj)
        {
            obj.Id = row.FieldValue("Id", obj.Id);
            obj.Amount = row.FieldValue<decimal>("Amount", Decimal.MinValue);
            obj.Format = row.FieldValue<string>("Format", obj.Format);
            obj.HTML = row.FieldValue<string>("HTML", obj.HTML);

            //Cache the newly loaded object
            ModelObjectCache.CacheObject(obj);
        }

        public UserEvent FindByTest(string testid)
        {
            UserEvent obj = new UserEvent();

            if (plannumber.Length.Equals(0))
            {
                throw new HttpException("testid.Length.Equals(0) on FindByTest");
            }

            using (SqlConnection cn = GetDefaultSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(FIND_UserEvent_BY_PLAN, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = cmd.Parameters.Add("@testid", SqlDbType.VarChar, 9);

                    prm.Value = testid;
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
