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
using Daybreak_Midnight.XLogic.BlackMarket;

using Daybreak_Midnight.Helpers;

using MonoMod.Utils;

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

        private static CustomGameData Campaign => (CustomGameData)CustomGameData.Instance;

        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), nameof(CustomGameData.LoadCampaign))]
        [HarmonyPatch(new Type[] {typeof(string), typeof(ulong)})]
        public static void XLogicDebug()
        {
            Console.WriteLine("test");

            Console.WriteLine("test2");

            CustomGameData customGameData = (CustomGameData)CustomGameData.Instance;

            var logicObj = customGameData.GetPrivateField("campaignLogic");

            var campaignLogic = (StoryGraph.StoryGraph)logicObj;

            foreach (var node in campaignLogic.nodes)
            {
                Console.WriteLine($"{node} Ports:");
                foreach (var port in node.Ports)
                {
                    Console.WriteLine($">>> {port.fieldName} : {port.ValueType.Name} <<<");
                    Console.WriteLine($">>>> Is dynamic : {port.IsDynamic} <<<<<");
                }
            }
        }*/

        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), "LoadInLogic")]
        public static void LoadInXLogicPostfix(CustomGameData customGameData)
        {
            Console.WriteLine("Loading in XLogic...");

            object logicObj = Campaign.GetPrivateField("campaignLogic");

            Console.WriteLine(logicObj);

            StoryGraph.StoryGraph campaignLogic = (StoryGraph.StoryGraph)logicObj;

            foreach (var node in campaignLogic.nodes)
            {
                Console.WriteLine($"{node.name} Ports:");
                foreach (var port in node.Ports)
                {
                    Console.WriteLine($">>> {port.fieldName} : {port.ValueType.Name} <<<");
                    Console.WriteLine($">>>> Is dynamic : {port.IsDynamic} <<<<<");
                }
            }

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

            Campaign.SetPrivateField("campaignLogic", campaignLogic);

            Console.WriteLine("Loaded in XLogic!");
        }*/

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MPGraphCreateNodeContextMenu))]
        [HarmonyPatch("Awake")]
        public static void LoadInXLogicNodesPostfix(MPGraphCreateNodeContextMenu __instance)
        {
            MethodInfo createButtonMethod = __instance.GetType()
                .GetMethod("CreateButton", BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo createNodeMethod = __instance.GetType()
                .GetMethod("CreateNode", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(MPGraphNode existingNode in __instance.nodePrefabs)
            {
                foreach(var xLogicNode in XLogicNodes)
                {
                    if(existingNode.GetType().Equals(xLogicNode.GetType()))
                    {
                        __instance.nodePrefabs.Remove(existingNode);
                    }
                }
            }

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

            List<MPGraphNode> customLogicNodes = new List<MPGraphNode>()
            {
                CreateRemoveCampaignNode(), CreateAddSoftwareNode()
            };

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
                    .id = "Remove";

                removeCampaignNoteComponent.ClearUnecessaryComponents();

                return removeCampaignNoteComponent;
            }

            MPGraphNode CreateAddSoftwareNode()
            {
                MPGraphNode AddMissionNode = __instance.allNodeTypes.FirstOrDefault(p => p.name == "GraphNodeMission");
                GameObject AddMissionNodeObject = AddMissionNode.gameObject;

                GameObject AddSoftwareNode = UnityEngine.Object.Instantiate(AddMissionNodeObject, xLogic.transform);

                AddSoftwareNode.name = "GraphAddSoftwareNode";

                var addMissionComponent = AddSoftwareNode.GetComponent<MPGraphNodeMission>();
                var addSoftwareComponent = AddSoftwareNode.AddComponent<MPGraphAddSoftware>();

                GameObject dropdownObj = AddSoftwareNode.transform.GetChild(5).gameObject;

                addSoftwareComponent.dropdown = dropdownObj.GetComponent<Dropdown>();

                GameObject selObj = AddSoftwareNode.transform.GetChild(4).gameObject;

                addSoftwareComponent.SetPrivateMPGraphNodeField("selected", selObj);

                addSoftwareComponent.title = AddSoftwareNode.transform.GetChild(1).gameObject.GetComponent<Text>();

                addSoftwareComponent.title.text = "ADD SOFTWARE TO B.M.";

                AddSoftwareNode.transform.GetChild(2).gameObject.GetComponent<Text>().text = "ADD";
                AddSoftwareNode.transform.GetChild(2).gameObject.GetComponent<MPGraphNodeInputPort>()
                    .id = "Add";

                addSoftwareComponent.ClearUnecessaryComponents();

                return addSoftwareComponent;
            }

            Console.WriteLine("Successfully created XLogic nodes! Adding their prefabs...");

            allNodeTypes.AddRange(customLogicNodes);

            __instance.allNodeTypes = allNodeTypes;

            XLogicNodes.AddRange(customLogicNodes);

            Console.WriteLine("XLogic node initialization complete.");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CustomGameData), "ConstructTranslationDictionary")]
        public static void XLogicTranslationsPostfix(CustomGameData __instance)
        {
            Console.WriteLine("Adding XLogic translations...");

            var xLogicTranslations = new Dictionary<Tuple<string, string>, string[]>
            {
                { new Tuple<string, string>(MPGraphNodeRemoveCampaignNote.ID, "Remove"), new string[1] { "remove" } },
                { new Tuple<string, string>(MPGraphNodeRemoveCampaignNote.ID, "Then"), new string[1] { "removed" } },
                { new Tuple<string, string>(MPGraphAddSoftware.ID, "Add"), new string[1] { "add" } },
                { new Tuple<string, string>(MPGraphAddSoftware.ID, "Complete"), new string[1] { "added" } }
            };

            var originalTranslations = (Dictionary<Tuple<string, string>, string[]>)__instance.GetPrivateStaticField("customNodePortTranslations");

            originalTranslations.AddRange(xLogicTranslations);

            __instance.SetPrivateStaticField("customNodePortTranslations", originalTranslations);

            Console.WriteLine("Added XLogic translations!");

            var newTranslations = (Dictionary<Tuple<string, string>, string[]>)__instance.GetPrivateStaticField("customNodePortTranslations");

            foreach(var translation in newTranslations)
            {
                Console.WriteLine($"{translation.Key.Item1}, {translation.Key.Item2}: {translation.Value[0]}");
            }
        }
    }
}
