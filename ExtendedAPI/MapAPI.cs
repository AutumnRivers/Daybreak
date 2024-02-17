using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Daybreak_Midnight.Static;

using Pinion;

using UnityEngine;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class MapAPI
    {
        private static bool Active => NodeMapAPI.NodeMapActive();

        [APIMethod]
        public static void ToggleExit(bool canExit = true)
        {
            if(Active)
            {
                GameObject consoleObj = GameObject.Find(ConstantVariables.CONSOLE_PATH);
                ConsoleController console = consoleObj.GetComponent<ConsoleController>();
                CyberspaceCommandParser commandParser = (CyberspaceCommandParser)console.commandParser;

                Command exitCommand = commandParser.Commands.FirstOrDefault(c => c.GetType() == typeof(JackOutCommand));

                int exitIndex = commandParser.Commands.IndexOf(exitCommand);

                commandParser.Commands[exitIndex].disabled = !canExit;
            }
        }

        [APIMethod]
        public static void DisableTrace(PinionContainer container)
        {
            if(Active)
            {
                TraceBar trace = (TraceBar)Game.Controller.Trace;

                trace.ManageThreatLevel = false;
                trace.Trace.MinValue = 0;
                trace.Trace.MaxValue = 0;
                trace.Trace.NotifyChange();
            }
        }

        [APIMethod]
        public static void RemoveICE(PinionContainer container, string nodeFrom, string nodeTo)
        {
            Node nodeF = GetNode(nodeFrom);
            Node nodeT = GetNode(nodeTo);

            NodeConnection nodeConnection = nodeF.connections.FirstOrDefault(c => c.To == nodeT);

            if(nodeConnection == null)
            {
                container.LogError("Couldn't find a connection between those nodes!");
                return;
            }

            ICE connectionICE = nodeConnection.ICE;

            Program killer = new Poison();

            connectionICE.GetType().GetMethod("Die", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(connectionICE, [killer]);
        }

        private static Node GetNode(string nodeId)
        {
            NodeMap nodeMap = Game.Controller.nodeMap;

            Node targetNode = nodeMap.Nodes.FirstOrDefault(n => n.Address == nodeId);

            return targetNode;
        }
    }
}
