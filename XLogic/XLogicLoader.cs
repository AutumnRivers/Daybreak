using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using HarmonyLib;
using MPGraph;
using StoryGraph;
using UnityEngine;
using XNode;

using Daybreak_Midnight.XLogic.CampaignNotes;
using Daybreak_Midnight.XLogic.BlackMarket;

using Daybreak_Midnight.Helpers;

namespace Daybreak_Midnight.XLogic
{
    [HarmonyPatch]
    public static class XLogicLoader
    {
        private static CustomGameData Campaign => (CustomGameData)CustomGameData.Instance;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CustomGameData), "LoadInLogic")]
        public static bool XLogic_LoadInLogic_Replacement(CustomGameData customGameData)
        {
            object containerObj = Campaign.GetPrivateField("contentContainer");

            CustomCampaignContentContainer contentContainer = (CustomCampaignContentContainer)containerObj;

            object nodeTranslationsObj = Campaign.GetPrivateStaticField("customNodePortTranslations");

            Dictionary<Tuple<string, string>, string[]> customNodePortTranslations =
                (Dictionary<Tuple<string, string>, string[]>)nodeTranslationsObj;

            StoryGraph.StoryGraph campaignLogic = ScriptableObject.CreateInstance<global::StoryGraph.StoryGraph>();
            string campaignLogicFilePath = CampaignEditorUtil.GetCampaignLogicFilePath(customGameData.CampaignID, customGameData.WorkshopID);
            if (!File.Exists(campaignLogicFilePath))
            {
                Debug.LogError("Could not find logic file");
                return false;
            }

            string s = File.ReadAllText(campaignLogicFilePath);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MPGraphData));
            StringReader stringReader = new StringReader(s);
            MPGraphData mPGraphData = (MPGraphData)xmlSerializer.Deserialize(stringReader);
            stringReader.Close();
            foreach (MPGraphNodeData node3 in mPGraphData.nodes)
            {
                if (node3.nodeID == MPGraphNodeCampaignStart.ID)
                {
                    campaignLogic.AddNode<StoryRootNode>();
                }
                else if (node3.nodeID == MPGraphNodeEmail.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString = node3 as MPGraphNodeDataString;
                    CustomEmail customEmail = contentContainer.GetCustomEmail(mPGraphNodeDataString.value);
                    if (customEmail == null)
                    {
                        CustomOutgoingEmail customOutgoingEmail = contentContainer.GetCustomOutgoingEmail(mPGraphNodeDataString.value);
                        if (customOutgoingEmail == null)
                        {
                            Debug.LogError("Could not find outgoing email: " + mPGraphNodeDataString.value);
                            continue;
                        }

                        OutgoingEmailNode outgoingEmailNode = campaignLogic.AddNode<OutgoingEmailNode>();
                        outgoingEmailNode.email = customOutgoingEmail;
                        outgoingEmailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "options 0");
                        outgoingEmailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "options 1");
                        outgoingEmailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "options 2");
                    }
                    else
                    {
                        EmailNode emailNode = campaignLogic.AddNode<EmailNode>();
                        emailNode.email = customEmail;
                        emailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "responses 0");
                        emailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "responses 1");
                        emailNode.AddDynamicOutput(typeof(TriggerPort[]), XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "responses 2");
                    }
                }
                else if (node3.nodeID == MPGraphNodeMission.ID)
                {
                    AddMissionNode addMissionNode = campaignLogic.AddNode<AddMissionNode>();
                    MPGraphNodeDataString mPGraphNodeDataString2 = node3 as MPGraphNodeDataString;
                    Mission customMission = contentContainer.GetCustomMission(mPGraphNodeDataString2.value);
                    addMissionNode.mission = customMission;
                }
                else if (node3.nodeID == MPGraphNodeAnd.ID)
                {
                    LogicNode logicNode = campaignLogic.AddNode<LogicNode>();
                    logicNode.AddDynamicInput(null, XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "input1");
                    logicNode.AddDynamicInput(null, XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.None, "input2");
                }
                else if (node3.nodeID == MPGraphNodeCampaignEnd.ID)
                {
                    campaignLogic.AddNode<CampaignEndNode>();
                }
                else if (node3.nodeID == MPGraphNodeAddCampaignNote.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString3 = node3 as MPGraphNodeDataString;
                    campaignLogic.AddNode<InternalAddCampaignNoteNode>().note = mPGraphNodeDataString3.value;
                }
                else if (node3.nodeID == MPGraphNodeHasCampaignNote.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString4 = node3 as MPGraphNodeDataString;
                    campaignLogic.AddNode<InternalHasCampaignNoteNode>().note = mPGraphNodeDataString4.value;
                }
                else if (node3.nodeID == MPGraphNodeCredits.ID)
                {
                    MPGraphNodeDataInt mPGraphNodeDataInt = node3 as MPGraphNodeDataInt;
                    campaignLogic.AddNode<AddCreditsNode>().amount = mPGraphNodeDataInt.amount;
                }
                else if (node3.nodeID == MPGraphNodeRemoveMission.ID)
                {
                    RemoveMissionNode removeMissionNode = campaignLogic.AddNode<RemoveMissionNode>();
                    MPGraphNodeDataString mPGraphNodeDataString5 = node3 as MPGraphNodeDataString;
                    Mission customMission2 = contentContainer.GetCustomMission(mPGraphNodeDataString5.value);
                    removeMissionNode.mission = customMission2;
                }
                else if (node3.nodeID == MPGraphNodeRelay.ID)
                {
                    campaignLogic.AddNode<RelayNode>();
                }
                else if (node3.nodeID == MPGraphNodeRemoveCampaignNote.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString3 = node3 as MPGraphNodeDataString;
                    var node = campaignLogic.AddNode<RemoveCampaignNoteNode>();
                    node.note = mPGraphNodeDataString3.value;
                } else if(node3.nodeID == MPGraphAddSoftware.ID)
                {
                    MPGraphNodeDataString mPGraphNodeDataString3 = node3 as MPGraphNodeDataString;
                    var node = campaignLogic.AddNode<AddSoftwareNode>();
                    node.programName = ProgramLookup.Programs[mPGraphNodeDataString3.value];
                }
            }

            foreach (MPGraphConnectionData connection in mPGraphData.connections)
            {
                if (connection.outputNodeIndex < 0 || connection.outputNodeIndex >= campaignLogic.nodes.Count || connection.inputNodeIndex < 0 || connection.inputNodeIndex >= campaignLogic.nodes.Count)
                {
                    continue;
                }

                XNode.Node node = campaignLogic.nodes[connection.outputNodeIndex];
                XNode.Node node2 = campaignLogic.nodes[connection.inputNodeIndex];
                if (node is AddMissionNode && node2 is CampaignEndNode)
                {
                    Debug.LogError("Mission node hooked up to ending");
                    ((node as AddMissionNode).mission as CustomMission).finalMission = true;
                    continue;
                }

                NodePort nodePort = null;
                NodePort nodePort2 = null;
                Tuple<string, string> key = new Tuple<string, string>(mPGraphData.nodes[connection.outputNodeIndex].nodeID, connection.outputID);
                if (!customNodePortTranslations.ContainsKey(key))
                {
                    Debug.LogError("No translation found for " + mPGraphData.nodes[connection.outputNodeIndex].nodeID + ", " + connection.outputID);
                    throw new InvalidCampaignSaveException();
                }

                string[] array = customNodePortTranslations[key];
                foreach (string fieldName in array)
                {
                    nodePort = node.GetOutputPort(fieldName);
                    if (nodePort != null)
                    {
                        break;
                    }
                }

                if (nodePort == null)
                {
                    Debug.LogError("Could not match output port for: " + mPGraphData.nodes[connection.outputNodeIndex].nodeID + ", " + connection.outputID);
                    Debug.LogError("Options are:");
                    foreach (NodePort port in node.Ports)
                    {
                        Debug.LogError(port.fieldName);
                    }

                    throw new InvalidCampaignSaveException();
                }

                key = new Tuple<string, string>(mPGraphData.nodes[connection.inputNodeIndex].nodeID, connection.inputID);
                if (!customNodePortTranslations.ContainsKey(key))
                {
                    Debug.LogError("Not translation found for input port: " + mPGraphData.nodes[connection.inputNodeIndex].nodeID + ", " + connection.inputID);
                    throw new InvalidCampaignSaveException();
                }

                array = customNodePortTranslations[key];
                foreach (string fieldName2 in array)
                {
                    nodePort2 = node2.GetInputPort(fieldName2);
                    if (nodePort2 != null)
                    {
                        break;
                    }
                }

                if (nodePort2 == null)
                {
                    Debug.LogError("Could not match input port for: " + mPGraphData.nodes[connection.inputNodeIndex].nodeID + ", " + connection.inputID);
                    Debug.LogError("Options are:");
                    foreach (NodePort port2 in node2.Ports)
                    {
                        Debug.LogError(port2.fieldName);
                    }

                    throw new InvalidCampaignSaveException();
                }

                nodePort.Connect(nodePort2);
            }

            Campaign.SetPrivateField("campaignLogic", campaignLogic);

            return false;
        }
    }
}
