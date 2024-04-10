using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub.Converters
{
    public interface INotificationConverter
    {
        object Convert(Notification notification);
    }
}
