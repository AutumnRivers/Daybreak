using System;
using System.Collections.Generic;
using System.Text;

using HarmonyLib;
using StoryGraph;

namespace Daybreak_Midnight.Patches
{
    [HarmonyPatch]
    public class AddCustomCampaignNote
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), "LoadInLogic")]
        static void CustomCampaignNotePostfix(CustomGameData __instance, ref CustomGameData customGameData)
        {
            List<string> notes = customGameData.progress.campaignNotes;

            bool hasNote = notes.Exists(s => s == "DaybreakUser");

            if(!hasNote)
            {
                notes.Add("DaybreakUser");
            }

            customGameData.progress.campaignNotes = notes;

            return;
        }
    }
}
