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
        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), nameof(CustomGameData.StartNewCustomCampaign))]
        static void CustomCampaignPostfix(CustomGameData __instance)
        {
            bool hasDaybreakNote = __instance.progress.campaignNotes.Exists(s => s == "DaybreakUser");

            if(!hasDaybreakNote)
            {
                __instance.progress.campaignNotes.Add("DaybreakUser");
            }
        }*/

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
