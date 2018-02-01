using System.Data;

namespace LIB.Extensions
{
    public static class EDataSet
    {
        public static bool IsTableFilled(this DataSet ds)
        {
            return ds.Tables["Table"] != null
                && ds.Tables["Table"].Rows.Count > 0;
        }
    }
}
