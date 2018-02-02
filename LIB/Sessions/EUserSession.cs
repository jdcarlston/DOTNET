using LIB.Extensions;
using System.Web;

namespace LIB.Data
{
    public static class EUserSession
    {
        public static void Load(this UserSession sn)
        {
            if (AppCache.Counter.Equals(AppCache.QPaths.Count - 1))
                AppCache.Counter = 0;
            else
                AppCache.Counter++;

            HttpContext.Current.Session.Timeout = 60;

            sn.QueuePath = AppCache.QPaths[AppCache.Counter];

            sn.SessionId = HttpContext.Current.Session.SessionID;
            sn.Agent = HttpContext.Current.Request.UserAgent;
            sn.Browser = HttpContext.Current.Request.Browser.Browser;
            sn.IpAddresses.Add(HttpContext.Current.Request.GetIpAddress());
            sn.UserEvents.Add(new UserEvent(sn.SessionId, "Start"));

            HttpContext.Current.Session["UserSession"] = sn;
        }
    }
}
