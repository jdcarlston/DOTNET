using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LIB.Extensions;
using System.Data;
using System.Xml;

namespace LIB.Data
{
    public enum Areas { DMZ, UAT, QA, DEV }

    public class AppCache
    {
        private const string GET_INPUTLISTS = "[dbo].[GetInputListsAsXml]";
        private const string GET_SITEPATHS = "[dbo].[GetSitePathsAsXml]";
        private const string GET_QPATHS = "[dbo].[GetQPathsAsXml]";

        private static AppCache _instance = new AppCache();

        private DateTime _ts = DateTime.Now;
        private Areas _area;
        private string _accessrole = string.Empty;
        private string _dbconnstr = string.Empty;
        private string _defaultdatabase = string.Empty;
        private string _smtp = string.Empty;
        private int _counter = 0;

        private List<KeyValuePair<string, string>> _sitepaths = new List<KeyValuePair<string, string>>();
        private QueuePaths _qpaths = new QueuePaths();
        private InputLists _inputlists = null;

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

        public static List<KeyValuePair<string, string>> SitePaths
        {
            get
            {
                if (null == _instance._inputlists)
                {
                    using (SqlConnection cn = new SqlConnection(DbConnStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(GET_SITEPATHS, cn))
                        {
                            cn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;

                            try
                            {
                                using (XmlReader dr = cmd.ExecuteXmlReader())
                                {
                                    _instance._sitepaths = dr.Deserialize<List<KeyValuePair<string, string>>>();
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }
                return _instance._sitepaths;
            }
            set { _instance._sitepaths = value; }
        }

        public static QueuePaths QPaths
        {
            get
            {
                if (_instance._qpaths.Count.Equals(0))
                {
                    if (null == _instance._qpaths)
                    {
                        using (SqlConnection cn = new SqlConnection(DbConnStr))
                        {
                            using (SqlCommand cmd = new SqlCommand(GET_QPATHS, cn))
                            {
                                cn.Open();
                                cmd.CommandType = CommandType.StoredProcedure;

                                try
                                {
                                    using (XmlReader dr = cmd.ExecuteXmlReader())
                                    {
                                        _instance._qpaths = dr.Deserialize<QueuePaths>();
                                    }
                                }
                                catch
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    //switch (Area)
                    //{
                    //    case Areas.DMZ:
                    //        _instance._qpaths.Add(new QueuePath("dmz_qin1", "dmz_qout1", "dmz_qack1"));
                    //        _instance._qpaths.Add(new QueuePath("dmz_qin2", "dmz_qout2", "dmz_qack2"));
                    //        break;
                    //    default:
                    //        _instance._qpaths.Add(new QueuePath("uat_qin1", "uat_qout1", "uat_qack1"));
                    //        _instance._qpaths.Add(new QueuePath("uat_qin2", "uat_qout2", "uat_qack2"));
                    //        break;
                    //}
                }
                return _instance._qpaths;
            }
            set { _instance._qpaths = value; }
        }

        public static InputLists InputLists
        {
            get
            {
                if (null == _instance._inputlists)
                {
                    using (SqlConnection cn = new SqlConnection(DbConnStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(GET_INPUTLISTS, cn))
                        {
                            cn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;

                            try
                            {
                                using (XmlReader dr = cmd.ExecuteXmlReader())
                                {
                                    _instance._inputlists = dr.Deserialize<InputLists>();
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }

                return _instance._inputlists;
            }
            set { _instance._inputlists = value; }
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
