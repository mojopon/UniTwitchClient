using UniTwitchClient.EventSub.WebSocket;

namespace UniTwitchClient.EventSub.Converters
{
    public interface INotificationConverter
    {
        object Convert(Notification notification);
    }
}
