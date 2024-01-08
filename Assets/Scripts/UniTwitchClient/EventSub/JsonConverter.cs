using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public static class JsonConverter
    {
        public static T ConvertFromJson<T>(string data) where T : class
        {
            return JsonUtility.FromJson<T>(data);
        }

        public static string ConvertToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
