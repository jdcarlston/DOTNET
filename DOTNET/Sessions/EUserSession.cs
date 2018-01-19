using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOTNET;
using System.Web;

namespace DOTNET.Data
{
    public static class EUserSession
    {
        public static void Load(this UserSession sn)
        {
            if (AppCache.Counter.Equals(AppCache.MsmqConns.Count - 1))
                AppCache.Counter = 0;
            else
                AppCache.Counter++;

            HttpContext.Current.Session.Timeout = 60;

            sn.MsmqConnection = AppCache.MsmqConns[AppCache.Counter];

            sn.SessionId = HttpContext.Current.Session.SessionID;
            sn.Agent = HttpContext.Current.Request.UserAgent;
            sn.Browser = HttpContext.Current.Request.Browser.Browser;
            sn.IpAddresses.Add(HttpContext.Current.Request.GetIpAddress());
            sn.UserEvents.Add("Session Start");

            HttpContext.Current.Session["UserSession"] = sn;
        }
    }
}
