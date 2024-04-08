using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public class Emote
    {
        public string Id { get; }
        public string Name { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }

        public Emote(string id, string name, int startIndex, int endIndex)
        {
            Name = name;
            Id = id;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
