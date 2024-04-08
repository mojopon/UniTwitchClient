using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public interface INotificationHandler
    {
        void HandleNotification(Notification notification);
    }
}