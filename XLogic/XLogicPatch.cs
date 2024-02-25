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
using Daybreak_Midnight.XLogic.UI;

using Daybreak_Midnight.Helpers;

using MonoMod.Utils;

namespace Daybreak_Midnight.XLogic
{
    /*
     * DAYBREAK - eXtended Logic (XLogic)
     * 
     * Extends Midnight Protocol's custom campaign logic system to allow for new, custom logic to be placed and parsed.
     */

    public class XLogic
    {
        public static void Log(string message)
        {
            Debug.Log("[Daybreak XLogic] " + message);
        }

        public static IEnumerable<Type> GetXLogicGraphNodes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach(Type type in assembly.GetTypes())
            {
                if(!type.IsSubclassOf(typeof(XLogicNode)))
                {
                    continue;
                }

                if(type.GetCustomAttributes(typeof(XLogicGraphNodeAttribute), false).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }

    [HarmonyPatch]
    public class XLogicPatch
    {
        private static readonly List<MPGraphNode> XLogicNodes = [];

        private static CustomGameData Campaign => (CustomGameData)CustomGameData.Instance;

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
                XLogic.Log($"Adding XLogic Node: {node.title.text}");

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

            #region add xlogic nodes
            var removeCampaignNode = CreateFreeformNode<XLogicRemoveCamapginNote>("GraphNodeRemoveCampaignNote",
                "REMOVE CAMPAIGN NOTE", "REMOVE", "Remove");

            var showPopupNode = CreateFreeformNode<XLogicShowPopup>("GraphNodeShowPopup", "SHOW POPUP", "SHOW", "Show");

            showPopupNode.inputField.characterLimit = 500;

            var addSoftwareNode = CreateDropdownNode<XLogicAddSoftware>("GraphAddSoftwareNode", "ADD SOFTWARE TO MARKET",
                "ADD", "Add");
            #endregion add xlogic nodes

            List<MPGraphNode> customLogicNodes = new List<MPGraphNode>()
            {
                removeCampaignNode, showPopupNode, addSoftwareNode
            };

            FreeformXLogicNode CreateFreeformNode<NodeType>(string nodeName, string title, string inputTitle, string inputID)
                where NodeType : FreeformXLogicNode
            {
                MPGraphNode AddCampaignNode = __instance.allNodeTypes.FirstOrDefault(p => p.name == "GraphNodeCampaignNote");
                GameObject AddCampaignNodeObject = AddCampaignNode.gameObject;

                GameObject FreeformNode = UnityEngine.Object.Instantiate(AddCampaignNodeObject, xLogic.transform);

                FreeformNode.name = nodeName;

                var addCampaignNoteComponent = FreeformNode.GetComponent<MPGraphNodeAddCampaignNote>();
                var removeCampaignNoteComponent = FreeformNode.AddComponent<NodeType>();

                addCampaignNoteComponent.enabled = false;

                GameObject selObj = FreeformNode.transform.GetChild(4).gameObject;

                removeCampaignNoteComponent.AddSelected(selObj);

                removeCampaignNoteComponent.title = FreeformNode.transform.GetChild(1).gameObject.GetComponent<Text>();

                removeCampaignNoteComponent.title.text = title;

                FreeformNode.transform.GetChild(2).gameObject.GetComponent<Text>().text = inputTitle;
                FreeformNode.transform.GetChild(2).gameObject.GetComponent<MPGraphNodeInputPort>()
                    .id = inputID;

                removeCampaignNoteComponent.inputField = removeCampaignNoteComponent.gameObject.transform.GetChild(5).gameObject
                    .GetComponent<InputField>();

                removeCampaignNoteComponent.ClearUnecessaryComponents();

                return removeCampaignNoteComponent;
            }

            DropdownXLogicNode CreateDropdownNode<NodeType>(string nodeName, string title, string inputTitle, string inputID)
                where NodeType : DropdownXLogicNode
            {
                MPGraphNode AddMissionNode = __instance.allNodeTypes.FirstOrDefault(p => p.name == "GraphNodeMission");
                GameObject AddMissionNodeObject = AddMissionNode.gameObject;

                GameObject DropdownNode = UnityEngine.Object.Instantiate(AddMissionNodeObject, xLogic.transform);

                DropdownNode.name = nodeName;

                var addMissionComponent = DropdownNode.GetComponent<MPGraphNodeMission>();
                var addSoftwareComponent = DropdownNode.AddComponent<NodeType>();

                GameObject dropdownObj = DropdownNode.transform.GetChild(5).gameObject;

                addSoftwareComponent.dropdown = dropdownObj.GetComponent<Dropdown>();

                GameObject selObj = DropdownNode.transform.GetChild(4).gameObject;

                addSoftwareComponent.AddSelected(selObj);

                addSoftwareComponent.title = DropdownNode.transform.GetChild(1).gameObject.GetComponent<Text>();

                addSoftwareComponent.title.text = title;

                DropdownNode.transform.GetChild(2).gameObject.GetComponent<Text>().text = inputTitle;
                DropdownNode.transform.GetChild(2).gameObject.GetComponent<MPGraphNodeInputPort>()
                    .id = inputID;

                addSoftwareComponent.ClearUnecessaryComponents();

                return addSoftwareComponent;
            }

            Console.WriteLine("Successfully created XLogic nodes! Adding their prefabs...");

            allNodeTypes.AddRange(customLogicNodes);

            __instance.allNodeTypes = allNodeTypes;

            XLogicNodes.Clear();

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
                { new Tuple<string, string>(XLogicRemoveCamapginNote.ID, "Remove"), new string[1] { "remove" } },
                { new Tuple<string, string>(XLogicRemoveCamapginNote.ID, "Then"), new string[1] { "removed" } },
                { new Tuple<string, string>(XLogicAddSoftware.ID, "Add"), new string[1] { "add" } },
                { new Tuple<string, string>(XLogicAddSoftware.ID, "Then"), new string[1] { "added" } },
                { new Tuple<string, string>(XLogicShowPopup.ID, "Show"), new string[1] { "show" } },
                { new Tuple<string, string>(XLogicShowPopup.ID, "Then"), new string[1] { "showed" } }
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
