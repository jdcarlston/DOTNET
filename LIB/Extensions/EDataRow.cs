using System;
using System.Data;
using System.Xml;

namespace LIB.Extensions
{
    public static class EDataRow
    {
        public static T FieldValue<T>(this DataRow row, string columnname, T defaultvalue) where T : IComparable
        {
            return (row.Table.Columns.Contains(columnname) && row[columnname] != System.DBNull.Value) ? (T)row[columnname] : defaultvalue;
        }
        public static XmlDocument FieldValue(this DataRow row, string columnname, XmlDocument doc)
        {
            if (row.Table.Columns.Contains(columnname) && row[columnname] != System.DBNull.Value)
            {
                doc.LoadXml((string)row[columnname]);
            }

            return doc;
        }
    }
}
