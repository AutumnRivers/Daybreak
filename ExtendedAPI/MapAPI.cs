using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
