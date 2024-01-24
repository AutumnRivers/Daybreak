using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Text;

using Pinion;

using Unity;
using UnityEngine;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class SysOpAPI
    {
        private static bool Active => NodeMapAPI.NodeMapActive();

        private static Dictionary<string, SysOp> sysOpIds = [];

        [APIMethod]
        public static void TeleportSysOpToNode(PinionContainer container, string nodeName, string sysOpId = "")
        {
            if(nodeName == null || nodeName == "")
            {
                container.LogError("Cannot have empty node name!");
                return;
            }

            if (Active)
            {
                Node targetNode = GetNode(nodeName);
                if (targetNode == null)
                {
                    container.LogError("Node not found!");
                    return;
                }

                SysOp targetSysOp = GetSysOp(sysOpId);
                if (targetSysOp == null)
                {
                    container.LogError("SysOp not found!");
                    return;
                }

                targetSysOp.TeleportToNode(targetNode);
            }
        }

        [APIMethod]
        public static bool SetSysOpId(PinionContainer container, string currentNode, string sysOpId)
        {
            if(currentNode == null || currentNode == "" || sysOpId == null || sysOpId == "")
            {
                container.LogError("Cannot have empty node or SysOp ID!");
                return false;
            }

            SysOp targetSysOp = GameObject.FindObjectsOfType<SysOp>()?.First(s => s.CurrentNode.Address == currentNode);

            if(targetSysOp == null)
            {
                container.LogError("Couldn't find SysOp! It's recommended to run SetSysOpId on MAP LOAD!");
                return false;
            }

            sysOpIds[sysOpId] = targetSysOp;

            return true;
        }

        [APIMethod]
        public static void SetSysOpTarget(PinionContainer container, string targetNode = "", string sysOpId = "")
        {
            string targetNodeAddress = targetNode;

            if(targetNode == "" || targetNode == "player")
            {
                targetNodeAddress = Game.Controller.NodeMap.CurrentNode.Address;
            }

            Node node = GetNode(targetNodeAddress);
            if (node == null)
            {
                if(targetNode == "player")
                {
                    node = Game.Controller.NodeMap.CurrentNode;
                } else
                {
                    container.LogError("Node not found!");
                    return;
                }
            }

            SysOp targetSysOp = GetSysOp(sysOpId);
            if (targetSysOp == null)
            {
                container.LogError("SysOp not found!");
                return;
            }

            targetSysOp.LastKnownLocation = node;
            targetSysOp.randomWalk = false;


            FloatingTextManager.Instance.SpawnRedactedText(node.transform, "-- SYSOP TARGET SET --");
        }

        private static SysOp GetSysOp(string sysOpId = "")
        {
            SysOp targetSysOp;

            if (sysOpId == "")
            {
                targetSysOp = GameObject.Find("DefaultSysOp(Clone)")?.GetComponent<SysOp>();
            }
            else
            {
                targetSysOp = sysOpIds[sysOpId];
            }

            return targetSysOp;
        }

        private static Node GetNode(string nodeId)
        {
            NodeMap nodeMap = Game.Controller.nodeMap;

            Node targetNode = nodeMap.Nodes.FirstOrDefault(n => n.Address == nodeId);

            return targetNode;
        }

        public static void SetPrivateField<T, T2>(this T item, string fieldName, T2 newValue)
        {
            var field = item.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(item, newValue);
        }
    }
}
