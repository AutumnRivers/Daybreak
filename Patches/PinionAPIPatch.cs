using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using HarmonyLib;

using Pinion;

namespace Daybreak_Midnight.Patches
{
    [HarmonyPatch]
    public class PinionAPIPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Pinion.PinionAPI), "GetAllAPISources")]
        static IEnumerable<Type> PinionAPISourcesPostfix(IEnumerable<Type> pinionTypes)
        {
            foreach(Type type in pinionTypes)
            {
                if (type.GetCustomAttribute(typeof(APISourceAttribute), inherit: false) != null)
                {
                    yield return type;
                }
            }

            Type[] daybreakTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach(Type type in daybreakTypes)
            {
                if (type.GetCustomAttribute(typeof(APISourceAttribute), inherit: false) != null)
                {
                    Console.WriteLine("Registering Custom API Method: " + type.Name);
                    yield return type;
                }
            }
        }
    }
}
