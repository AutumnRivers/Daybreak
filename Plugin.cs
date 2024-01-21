using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using BepInEx;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using Daybreak_Midnight.Static;

namespace Daybreak_Midnight
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("MidnightProtocol.exe")]
    public class Daybreak : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "autumnrivers.daybreak";
        public const string PLUGIN_NAME = "Daybreak";
        public const string PLUGIN_VERSION = "0.0.1";

        public static Harmony Harmony { get; } = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());  

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            Console.WriteLine("/-------------------------------------\\");
            Console.WriteLine("               _DAYBREAK               ");
            Console.WriteLine("       And so, our will goes on.       ");
            Console.WriteLine("         Autumn Rivers (c)2024         ");
            Console.WriteLine("\\-------------------------------------/");
        }
    }

    [HarmonyPatch]
    public class MainPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BootMainMenuController), "Start")]
        public static void BMPostfix()
        {
            Console.WriteLine("Booted into main menu!");
            GameObject canvas = GameObject.Find("Canvas");

            SetMainMenuVersion();

            // Shows the main pop-up during boot
            Popup.ShowMessage(Daybreak.PLUGIN_NAME, ConstantVariables.OPENING_WARNING);

            AddManualEntries(canvas);

            BootAudioOptionsController bootAudio = canvas.transform.Find("AudioOptions").
                GetComponent<BootAudioOptionsController>();

            MusicManager.musicVolume = SystemData.Instance.musicVolume;
        }

        private static void SetMainMenuVersion()
        {
            GameObject versionText = GameObject.Find("Canvas/Text");

            Text versionTextModule = versionText.GetComponent<Text>();

            versionTextModule.verticalOverflow = VerticalWrapMode.Overflow;
            versionTextModule.text += $"\n{Daybreak.PLUGIN_NAME} v{Daybreak.PLUGIN_VERSION}";
        }

        private static void AddManualEntries(GameObject canvas)
        {
            GameObject manual = canvas.transform.Find("Manual").gameObject;

            BootManualController manualModule = manual.GetComponent<BootManualController>();

            ManualManager.controller = manualModule;

            // Add About Section
            ManualManager.AddNewEntry($"About {Daybreak.PLUGIN_NAME}",
                CustomFilter.Filter(ConstantVariables.MANUAL_ENTRY), 0);

            // Add Music Usage
            ManualManager.AddNewEntry($"{Daybreak.PLUGIN_NAME} - Custom BGM",
                CustomFilter.Filter(ConstantVariables.BGM_MANUAL), 1);
        }
    }

    public class CustomFilter
    {
        public static string Filter(string s)
        {
            StringBuilder sb = new StringBuilder(s);

            sb.Replace("<h1>", "<size=40px><b>");
            sb.Replace("</h1>", "</b></size>");
            sb.Replace("<h2>", "<size=30px><b>");
            sb.Replace("</h2>", "</b></size>");

            return sb.ToString();
        }
    }
}
