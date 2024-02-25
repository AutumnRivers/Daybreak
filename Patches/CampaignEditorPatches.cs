using System;
using System.Collections.Generic;
using System.Text;

using HarmonyLib;

using UnityEngine;

using MPGraph;

using Daybreak_Midnight.Helpers;
using static UnityEngine.UI.Image;

namespace Daybreak_Midnight.Patches
{
    [HarmonyPatch]
    public class CampaignEditorPatches
    {
        // This is WIP code to fix an issue with the way logic is loaded across campaigns
        // It's broken, and doesn't properly work yet. only uncomment if you are prepared for Hell
        /*private static MPGraphConnection[] connections;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EditCampaignController),"ShowLogic")]
        public static void FixLogicGraphLoadPostfix(EditCampaignController __instance)
        {
            CampaignLogicGraph logicGraph = (CampaignLogicGraph)__instance.nodeGraph;
            logicGraph.LoadGraph();

            RectTransform origin = logicGraph.origin;

            Array.Clear(connections, 0, connections.Length);

            connections = origin.GetComponentsInChildren<MPGraphConnection>();

            foreach(var connection in connections)
            {
                connection.UpdateConnection();
            }
        }*/
    }
}
