using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models.Raws
{
    [Serializable]
    public class create_event_sub_request
    {
        public string type;
        public string version;
        public condition condition;
        public transport transport;
    }
}