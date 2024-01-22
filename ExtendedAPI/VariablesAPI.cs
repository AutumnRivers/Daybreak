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
    }
}
