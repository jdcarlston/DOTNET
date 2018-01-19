using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOTNET;

namespace DOTNET.Data
{
    public static class EUserEvent
    {
        public static void Log(this UserEvent e)
        {
            MUserEvents cm = (MUserEvent)ModelRegistry.Model(typeof(UserEvent));
            cm.UpdateEvents(id, e, sessionid);
        }
    }
}
