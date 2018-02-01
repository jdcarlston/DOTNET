using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Data
{
    public class MTest : Model<Test>
    {
        public const string FIND_TEST_BY_PLAN = "[dbo].[GetTestByPlanNumber]";

        public MTest() : base() { }

        internal override void LoadLine(DataRow row, Test obj)
        {
            obj.Id = row.FieldValue("Id", obj.Id);
            obj.Amount = row.FieldValue<decimal>("Amount", Decimal.MinValue);
            obj.Format = row.FieldValue<string>("Format", obj.Format);
            obj.HTML = row.FieldValue<string>("HTML", obj.HTML);

            //Cache the newly loaded object
            ModelObjectCache.CacheObject(obj);
        }

        public Test FindByPlanNumber(string plannumber)
        {
            Test obj = new Test();

            if (plannumber.Length.Equals(0))
            {
                throw new HttpException("test.Length.Equals(0) on FindByPlanNumber");
            }

            using (SqlConnection cn = GetDefaultSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(FIND_TEST_BY_PLAN, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = cmd.Parameters.Add("@routingnumber", SqlDbType.VarChar, 9);

                    prm.Value = plannumber;
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
