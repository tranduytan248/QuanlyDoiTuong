namespace Core.Inv.Logs
{
    public class InvLogger
    {
        private static readonly InvLogWriter invLogger = InvLogWriter.Instance;

        public static void LogAction(string userDoing, string actionCode, string contentMessage)
        {
            invLogger.WriteToLog($"[{userDoing}-{actionCode}] --- {contentMessage}");
        }
    }
}