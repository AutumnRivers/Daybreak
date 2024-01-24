using System;
using System.Collections.Generic;
using System.Text;

using Pinion;
using UnityEngine;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class VariablesAPI
    {
        private static Dictionary<string, string> globalVariables = new Dictionary<string, string>();
        private static Dictionary<string, int> globalCounter = new Dictionary<string, int>();

        [APIMethod]
        public static void SetGlobalVariable(PinionContainer _, string variableName, string value)
        {
            if(!globalVariables.ContainsKey(variableName))
            {
                globalVariables.Add(variableName, value);
            } else
            {
                globalVariables[variableName] = value;
            }
        }

        [APIMethod]
        public static string GetGlobalVariable(PinionContainer _, string variableName)
        {
            if (!globalVariables.ContainsKey(variableName))
            {
                return "";
            }
            else
            {
                return globalVariables[variableName];
            }
        }

        [APIMethod]
        public static int SetCounter(PinionContainer container, string counterName, int value)
        {
            if(counterName == null || counterName == "")
            {
                container.LogError("Cannot have empty name!");
                return -1;
            }

            if(globalCounter.ContainsKey(counterName))
            {
                globalCounter[counterName] = value;
            } else
            {
                globalCounter.Add(counterName, value);
            }

            return value;
        }

        [APIMethod]
        public static int AddCounter(PinionContainer container, string counterName)
        {
            if (counterName == null || counterName == "")
            {
                container.LogError("Cannot have empty name!");
                return -1;
            }

            if (globalCounter.ContainsKey(counterName))
            {
                globalCounter[counterName]++;
            }
            else
            {
                globalCounter.Add(counterName, 1);
            }

            return globalCounter[counterName];
        }

        [APIMethod]
        public static int SubtractCounter(PinionContainer container, string counterName)
        {
            if (counterName == null || counterName == "")
            {
                container.LogError("Cannot have empty name!");
                return -1;
            }

            if (globalCounter.ContainsKey(counterName))
            {
                globalCounter[counterName]--;
            }
            else
            {
                globalCounter.Add(counterName, 0);
            }

            return globalCounter[counterName];
        }

        [APIMethod]
        public static int GetCounter(PinionContainer container, string counterName)
        {
            if (counterName == null || counterName == "")
            {
                container.LogError("Cannot have empty name!");
                return -1;
            }

            if(globalCounter.ContainsKey(counterName))
            {
                return globalCounter[counterName];
            } else
            {
                container.LogError("Counter doesn't exist! Did you forget to set it?");
                return -1;
            }
        }
    }
}
