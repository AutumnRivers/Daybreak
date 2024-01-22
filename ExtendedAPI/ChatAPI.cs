using System;
using System.Collections;
using System.Collections.Generic;

using Pinion;

using UnityEngine;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class ChatAPI
    {
        private static bool Active => NodeMapAPI.NodeMapActive();
        private static NetworkChat Chat => Game.Controller.NetworkChat;

        private static int lastChoiceIndex;

        [APIMethod]
        public static void SendChatText(PinionContainer container, string text, float delay, string character = null)
        {
            if(text == null || text == "") { container.LogError("Cannot have empty message!"); }
            if(delay < 0f) { container.LogError("Delay cannot be shorter than zero seconds!"); }

            character ??= Translate.Me("General/System", "System");

            if(Active) {
                Chat.Talk(character, text, delay);
            }
        }

        [APIMethod]
        public static void SendPlayerText(PinionContainer container, string text, float delay)
        {
            if (text == null || text == "") { container.LogError("Cannot have empty message!"); }
            if (delay < 0f) { container.LogError("Delay cannot be shorter than zero seconds!"); }

            if(Active)
            {
                Chat.PlayerSayCommand(text);

                Chat.onInputSubmitted += DisableChatInput;
            }
        }

        [APIMethod]
        public static void ShowPlayerChoices(PinionContainer container, string choice1, string choice2 = null, string choice3 = null)
        {
            if(choice1 == null && choice2 == null && choice3 == null)
            {
                container.LogError("You need to have at least one choice!");
            }

            List<string> choices = new List<string>();

            if (choice1 != null && choice1 != "") { choices.Add(choice1); }
            if (choice2 != null && choice2 != "") { choices.Add(choice2); }
            if (choice3 != null && choice3 != "") { choices.Add(choice3); }

            if(Active)
            {
                string[] choicesArray = choices.ToArray();

                Chat.GainFocus();

                Game.Controller.StartCoroutine(
                    Chat.PlayerChose(choicesArray, 0.5f, SaveLastChoice)
                );
            }
        }

        [APIMethod]
        public static bool ChatIsActive()
        {
            return Chat.EnableInput;
        }

        [APIMethod]
        public static int LastChoice()
        {
            return lastChoiceIndex;
        }

        private static void DisableChatInput(int _)
        {
            Chat.EnableInput = false;
            Chat.onInputSubmitted -= DisableChatInput;
        }

        private static void SaveLastChoice(int lastChoice)
        {
            Console.WriteLine(lastChoice);
            lastChoiceIndex = lastChoice;
            Chat.EnableInput = false;
        }
    }
}
