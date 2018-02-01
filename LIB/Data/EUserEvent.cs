namespace LIB.Data
{
    public static class EUserEvent
    {
        public static void Log(this UserEvent e, UserSession sn)
        {
            e.SessionId = sn.SessionId;
            sn.UserEvents.Add(e);

            MUserEvent em = (MUserEvent)ModelRegistry.Model(typeof(UserEvent));
            em.Log(e);
        }
    }
}
