using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using DOTNET;
using System.Data;
using System.Xml;
using System.Web;

namespace DOTNET.Data
{
    public enum Areas { LIVE, UAT, DEV }

    public class AppCache
    {
        private static AppCache _instance = new AppCache();

        private DateTime _ts = DateTime.Now;
        private Areas _area;
        private string _accessrole = string.Empty;
        private string _dbconnstr = string.Empty;
        private string _database = string.Empty;
        private string _smtp = string.Empty;
        private int _counter = 0;
        private List<KeyValuePair<string, string>> _paths = new List<KeyValuePair<string, string>>();

        public AppCache()
        { }

        public static DateTime TS
        {
            get { return _instance._ts; }
            set { _instance._ts = value; }
        }

        public static int Counter
        {
            get { return _instance._counter; }
            set { _instance._counter = value; }
        }
        

        public static Areas Area
        {
            get
            {
                if (Regex.IsMatch(Environment.MachineName.ToUpper(), @""))
                    _instance._area = Areas.LIVE;
                else if (Regex.IsMatch(Environment.MachineName.ToUpper(), @""))
                    _instance._area = Areas.UAT;
                else
                    _instance._area = Areas.DEV;

                return _instance._area;
            }
            set { _instance._area = value; }
        }

        public static string DbConnStr
        {
            get
            {
                if (_instance._dbconnstr.IsNullOrEmpty())
                {
                    switch (Area)
                    {
                        case Areas.LIVE: _instance._dbconnstr = WebConfigurationManager.ConnectionStrings["LIVE_DB"].ConnectionString.Decrypt(); break;
                        default: _instance._dbconnstr = WebConfigurationManager.ConnectionStrings["UAT_DB"].ConnectionString; break;
                    }
                }

                return _instance._dbconnstr;
            }
            set { _instance._dbconnstr = value; }
        }

        public static string Database
        {
            get
            {
                if (_instance._database.IsNullOrEmpty())
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(DbConnStr);
                    _instance._database = !builder.InitialCatalog.IsNullOrEmpty() ? builder.InitialCatalog : "";
                }

                return _instance._database;
            }
            set { _instance._database = value; }
        }

        public static string SMTP
        {
            get
            {
                if (_instance._smtp.IsNullOrEmpty())
                {
                    switch (Area)
                    {
                        case Areas.LIVE: _instance._smtp = WebConfigurationManager.AppSettings["LIVE_SMTP"].ToString(); break;
                        default: _instance._smtp = WebConfigurationManager.AppSettings["DEV_SMTP"].ToString(); break;
                    }
                }

                return _instance._smtp;
            }
            set { _instance._smtp = value; }
        }

        public static SmtpClient SmtpClient()
        {
            if (SMTP == null)
                return new SmtpClient(AppCache.SMTP);

            return new SmtpClient();
        }
        
    }
}
