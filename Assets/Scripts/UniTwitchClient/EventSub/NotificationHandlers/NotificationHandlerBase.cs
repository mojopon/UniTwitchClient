using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

public abstract class NotificationHandlerBase<T> : INotificationHandler
{
    public abstract void HandleNotification(Notification notification);

    protected Action<T> _onHandle;

    public NotificationHandlerBase(Action<T> onHandle) 
    {
        _onHandle = onHandle;
    }
}
