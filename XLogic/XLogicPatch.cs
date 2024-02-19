using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using System.Linq;

using HarmonyLib;

using MPGraph;
using StoryGraph;

using XNode;

using UnityEngine;
using UnityEngine.UI;

using Daybreak_Midnight.XLogic.CampaignNotes;

using Daybreak_Midnight.Helpers;

namespace Daybreak_Midnight.XLogic
{
    /*
     * DAYBREAK - eXtended Logic (XLogic)
     * 
     * Extends Midnight Protocol's custom campaign logic system to allow for new, custom logic to be placed and parsed.
     */

    [HarmonyPatch]
    public class XLogicPatch
    {
        private static readonly List<MPGraphNode> XLogicNodes = [];

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), "LoadInLogic")]
        public static void LoadInXLogicPostfix(ref CustomGameData customGameData)
        {
            PropertyInfo logicProp = customGameData.GetType()
                .GetProperty("campaignLogic", BindingFlags.NonPublic | BindingFlags.Instance);

            object logicObj = logicProp.GetGetMethod(nonPublic: true).Invoke(customGameData, null);

            StoryGraph.StoryGraph campaignLogic = (StoryGraph.StoryGraph)logicObj;

            campaignLogic = ScriptableObject.CreateInstance<global::StoryGraph.StoryGraph>();
            string campaignLogicFilePath = CampaignEditorUtil.GetCampaignLogicFilePath(customGameData.CampaignID, customGameData.WorkshopID);
            if (!File.Exists(campaignLogicFilePath))
            {
                Debug.LogError("Could not find logic file");
                return;
            }

            string s = File.ReadAllText(campaignLogicFilePath);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MPGraphData));
            StringReader stringReader = new StringReader(s);
            MPGraphData mPGraphData = (MPGraphData)xmlSerializer.Deserialize(stringReader);
            stringReader.Close();

            foreach (MPGraphNodeData node3 in mPGraphData.nodes)
            {
                if (node3.nodeID == MPGraphNodeRemoveCampaignNote.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString3 = node3 as MPGraphNodeDataString;
                    campaignLogic.AddNode<RemoveCampaignNoteNode>().note = mPGraphNodeDataString3.value;
                }
            }

            customGameData.GetType().GetProperty("campaignLogic", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(customGameData, campaignLogic);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MPGraphCreateNodeContextMenu))]
        [HarmonyPatch("Awake")]
        public static void LoadInXLogicNodesPostfix(MPGraphCreateNodeContextMenu __instance)
        {
            MethodInfo createButtonMethod = __instance.GetType()
                .GetMethod("CreateButton", BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo createNodeMethod = __instance.GetType()
                .GetMethod("CreateNode", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MPGraphNode node in XLogicNodes)
            {
                Console.WriteLine($"Adding XLogic Node: {node.title.text}");

                Button nodeButton = (Button)createButtonMethod.Invoke(__instance, [node.title.text]);

                nodeButton.onClick.AddListener(delegate
                {
                    createNodeMethod.Invoke(__instance, [node]);
                });

                __instance.nodePrefabs.Add(node);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MPGraphCanvas),"Awake")]
        public static void LoadInXLogicPrefabsPostfix(MPGraphCanvas __instance)
        {
            List<MPGraphNode> allNodeTypes = __instance.allNodeTypes;

            GameObject xLogicObj = GameObject.Find("DaybreakXLogic");

            if (xLogicObj == null)
            {
                GameObject xLogicGameObject = new("DaybreakXLogic")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                xLogicObj = xLogicGameObject;
            }

            GameObject xLogic = GameObject.Instantiate(xLogicObj);

            Console.WriteLine("Loading in XLogic nodes...");

            List<MPGraphNode> customLogicNodes = [CreateRemoveCampaignNode()];

            MPGraphNode CreateRemoveCampaignNode()
            {
                MPGraphNode AddCampaignNode = __instance.allNodeTypes.FirstOrDefault(p => p.name == "GraphNodeCampaignNote");
                GameObject AddCampaignNodeObject = AddCampaignNode.gameObject;

                GameObject RemoveCampaignNode = UnityEngine.Object.Instantiate(AddCampaignNodeObject, xLogic.transform);

                RemoveCampaignNode.name = "GraphNodeRemoveCampaignNote";

                var addCampaignNoteComponent = RemoveCampaignNode.GetComponent<MPGraphNodeAddCampaignNote>();
                var removeCampaignNoteComponent = RemoveCampaignNode.AddComponent<MPGraphNodeRemoveCampaignNote>();

                addCampaignNoteComponent.enabled = false;

                GameObject selObj = RemoveCampaignNode.transform.GetChild(4).gameObject;

                removeCampaignNoteComponent.SetPrivateMPGraphNodeField<MPGraphNode, GameObject>("selected", selObj);

                removeCampaignNoteComponent.inputField = RemoveCampaignNode.transform.GetChild(5).gameObject
                    .GetComponent<InputField>();

                removeCampaignNoteComponent.title = RemoveCampaignNode.transform.GetChild(1).gameObject.GetComponent<Text>();

                removeCampaignNoteComponent.title.text = "REMOVE CAMPAIGN NOTE";

                RemoveCampaignNode.transform.GetChild(2).gameObject.GetComponent<Text>().text = "REMOVE";
                RemoveCampaignNode.transform.GetChild(2).gameObject.GetComponent<MPGraphNodeInputPort>()
                    .id = "REMOVE";

                return removeCampaignNoteComponent;
            }

            Console.WriteLine("Successfully created XLogic nodes! Adding their prefabs...");

            allNodeTypes.AddRange(customLogicNodes);

            __instance.allNodeTypes = allNodeTypes;

            XLogicNodes.AddRange(customLogicNodes);

            Console.WriteLine("XLogic node initialization complete.");
        }
    }
}
