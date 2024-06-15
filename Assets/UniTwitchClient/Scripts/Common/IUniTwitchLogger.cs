namespace UniTwitchClient.Common
{
    public interface IUniTwitchLogger
    {
        void Log(string message);
        void LogError(string message);
    }
}