using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscription
    {
        public string Id { get; private set; }

        public EventSubSubscription(string id) 
        {
            Id = id;
        }
    }
}
