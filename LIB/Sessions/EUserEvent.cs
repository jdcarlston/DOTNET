namespace LIB.Data
{
    public static class EUserEvent
    {
        public static void Log(this UserEvent e, int id, int sessionid)
        {
            MUserEvent cm = (MUserEvent)ModelRegistry.Model(typeof(UserEvent));
            //cm.UpdateEvents(id, e, sessionid);
        }
    }
}
