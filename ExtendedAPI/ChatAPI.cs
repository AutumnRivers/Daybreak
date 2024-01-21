using System;
using System.Collections.Generic;
using System.Text;

using BepInEx;

using Pinion;

using UnityEngine;

using Daybreak_Midnight.Static;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class ChatAPI
    {
        [APIMethod]
        public static void SendChatText(PinionContainer container, string text, float delay, string character = null)
        {
            if(text == null || text == "") { container.LogError("Cannot have empty message!"); }
            if(delay < 0f) { container.LogError("Delay cannot be shorter than zero seconds!"); }

            character ??= Translate.Me("General/System", "System");

            if(NodeMapAPI.NodeMapActive()) {
                NetworkChat chat = Game.Controller.NetworkChat;

                chat.Talk(character, text, delay);
            }
        }
    }
}
