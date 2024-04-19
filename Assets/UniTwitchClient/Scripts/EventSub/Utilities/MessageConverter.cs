using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket.Models.Raws;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static PlasticGui.WorkspaceWindow.CodeReview.Summary.CommentSummaryData;

namespace UniTwitchClient.EventSub
{
    public class MessageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(message);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var message = new message();

            if (reader.TokenType == JsonToken.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndObject) break;
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        var prop = reader.Value?.ToString();
                        reader.Read();
                        switch (prop)
                        {
                            case "text":
                                message.text = (string)reader.Value;
                                break;
                            case "emotes":
                                message.emotes = new JsonSerializer().Deserialize<List<emote>>(reader);
                                break;
                        }
                    }
                }
            }
            else if (reader.TokenType == JsonToken.String)
            {
                message.text = (string)reader.Value;
            }

            return message;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}