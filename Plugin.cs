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
        public static void BMPostfix(BootMainMenuController __instance)
        {
            Console.WriteLine("Booted into main menu!");
            GameObject canvas = GameObject.Find("Canvas");

            SetMainMenuVersion();

            // Shows the main pop-up during boot
            Popup.ShowMessage(Daybreak.PLUGIN_NAME, ConstantVariables.OPENING_WARNING);

            AddManualEntries(canvas);

            AddToMainMenu(__instance);

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
        }

        private static void AddToMainMenu(BootMainMenuController mainMenu)
        {
            GameObject apiDocsPanel = AddAPIDocs();

            BootMainMenuController.MenuOption apiOption = new()
            {
                label = "Daybreak API",
                disableInDemo = true,
                panel = apiDocsPanel
            };

            apiOption.SetActionToDefaultNavigation();

            mainMenu.options.Insert(5, apiOption);

            mainMenu.Rebuild();
        }

        private static GameObject AddAPIDocs()
        {
            GameObject baseManual = GameObject.Find("Canvas").transform.Find("Manual").gameObject;
            GameObject canvas = GameObject.Find("Canvas");
            GameObject manualObj = GameObject.Instantiate(baseManual, canvas.transform).gameObject;

            manualObj.name = "DaybreakAPI";

            BootManualController manual = manualObj.GetComponent<BootManualController>();

            for(var idx = manual.transform.GetChild(0).childCount - 1; idx >= 0; idx--)
            {
                UnityEngine.Object.Destroy(manual.transform.GetChild(0).GetChild(idx).gameObject);
            }

            manual.localizedName = "Daybreak API";
            manual.entries.Clear();

            AddAPIDocEntries(manual);

            manual.Rebuild();

            return manualObj;
        }

        private static void AddAPIDocEntries(BootManualController manual)
        {
            ManualManager.AddNewEntryToManual($"About {Daybreak.PLUGIN_NAME}",
                CustomFilter.Filter(ConstantVariables.MANUAL_ENTRY), manual, 0);

            ManualManager.AddNewEntryToManual("Custom BGM", CustomFilter.Filter(APIDocs.BGM_MANUAL), manual);
            ManualManager.AddNewEntryToManual("Chat API - Basics", CustomFilter.Filter(APIDocs.CHAT1), manual);
            ManualManager.AddNewEntryToManual("Chat API - Choices", CustomFilter.Filter(APIDocs.CHAT_CHOICES), manual);
        }

        private static void NavigateToApiDocs(BootMainMenuController mainMenu, int index)
        {
            mainMenu.Navigate(index);
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
