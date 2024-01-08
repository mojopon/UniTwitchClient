using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models.Raws
{
    [Serializable]
    public class condition
    {
        public string broadcaster_user_id;
        public string user_id;
        public string moderator_user_id;
        public string from_broadcaster_user_id;
        public string to_broadcaster_user_id;
    }
}
