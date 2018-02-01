using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LIB.Extensions;

namespace LIB.Data
{
    public enum Areas { DMZ, UAT, QA, DEV }

    public class AppCache
    {
        private static AppCache _instance = new AppCache();

        private DateTime _ts = DateTime.Now;
        private Areas _area;
        private string _accessrole = string.Empty;
        private string _dbconnstr = string.Empty;
        private string _defaultdatabase = string.Empty;
        private string _smtp = string.Empty;
        private int _counter = 0;
        private List<KeyValuePair<string, string>> _paths = new List<KeyValuePair<string, string>>();
        private QueueConnections _qconns = new QueueConnections();

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
                    _instance._area = Areas.DMZ;
                else if (Regex.IsMatch(Environment.MachineName.ToUpper(), @""))
                    _instance._area = Areas.UAT;
                else
                    _instance._area = Areas.DEV;

                return _instance._area;
            }
            set { _instance._area = value; }
        }

        public static QueueConnections QConns
        {
            get
            {
                if (_instance._qconns.Count.Equals(0))
                {
                    switch (Area)
                    {
                        case Areas.DMZ:
                            _instance._qconns.Add(new QueueConnection("dmz_qin1", "dmz_qout1", "dmz_qack1"));
                            _instance._qconns.Add(new QueueConnection("dmz_qin2", "dmz_qout2", "dmz_qack2"));
                            break;
                        default:
                            _instance._qconns.Add(new QueueConnection("uat_qin1", "uat_qout1", "uat_qack1"));
                            _instance._qconns.Add(new QueueConnection("uat_qin2", "uat_qout2", "uat_qack2"));
                            break;
                    }
                }
                return _instance._qconns;
            }
            set { _instance._qconns = value; }
        }

        public static string DbConnStr
        {
            get
            {
                if (_instance._dbconnstr.IsNullOrEmpty())
                {
                    string dmz = "dmz_db";
                    string uat = "uat_db";
                    string qa = "qa_db";

                    switch (Area)
                    {
                        case Areas.DMZ: _instance._dbconnstr = dmz; break;
                        case Areas.UAT: _instance._dbconnstr = uat; break;
                        case Areas.QA: _instance._dbconnstr = qa; break;
                    }

                    if (_instance._dbconnstr.IsNullOrEmpty())
                        _instance._dbconnstr = uat;
                }

                if (_instance._dbconnstr.IsNullOrEmpty())
                    throw new Exception("No Default Database Connection set");

                return _instance._dbconnstr;
            }
            set { _instance._dbconnstr = value; }
        }

        public static string DefaultDatabase
        {
            get
            {
                if (_instance._defaultdatabase.IsNullOrEmpty())
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(DbConnStr);
                    _instance._defaultdatabase = !builder.InitialCatalog.IsNullOrEmpty() ? builder.InitialCatalog : "[Default]";
                    
                }

                return _instance._defaultdatabase;
            }
            set { _instance._defaultdatabase = value; }
        }

        public static SmtpClient SmtpClient()
        {
            if (SMTP == null)
                return new SmtpClient(AppCache.SMTP);

            return new SmtpClient();
        }

        public static string SMTP
        {
            get
            {
                if (_instance._smtp.IsNullOrEmpty())
                {
                    switch (Area)
                    {
                        case Areas.DMZ: _instance._smtp = "dmzmail"; break;
                        default: _instance._smtp = "uatmail"; break;
                    }
                }

                return _instance._smtp;
            }
            set { _instance._smtp = value; }
        }
    }
}
