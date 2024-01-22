using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BepInEx;

using UnityEngine;

namespace Daybreak_Midnight.Static
{
    public static class ManualManager
    {
        public static BootManualController controller;

        public static void AddNewEntry(string label, string description, int index = -1)
        {
            bool addToEnd = index < 0;

            if(controller.entries.Exists(i => i.label == label))
            {
                Debug.LogWarning("Tried to add duplicate entry to Manual - skipping.");
                return;
            }

            ManualItem newItem = ScriptableObject.CreateInstance<ManualItem>();
            newItem.label = label;
            newItem.name = label;
            newItem.explanation = description;

            if(addToEnd)
            {
                controller.entries.Add(newItem);
            } else
            {
                controller.entries.Insert(index, newItem);
            }

            Rebuild();
        }

        public static void AddNewEntryToManual(string label, string description, BootManualController manual, int index = -1)
        {
            bool addToEnd = index < 0;

            if (manual.entries.Exists(i => i.label == label))
            {
                Debug.LogWarning("Tried to add duplicate entry to Manual - skipping.");
                return;
            }

            ManualItem newItem = ScriptableObject.CreateInstance<ManualItem>();
            newItem.label = label;
            newItem.name = label;
            newItem.explanation = description;

            if (addToEnd)
            {
                manual.entries.Add(newItem);
            }
            else
            {
                manual.entries.Insert(index, newItem);
            }

            Rebuild();
        }

        public static void RemoveEntry(string label)
        {
            var entry = controller.entries.FirstOrDefault(i => i.label == label);

            if(entry == null) { return; }

            controller.entries.Remove(entry);

            Rebuild();
        }

        public static void Rebuild()
        {
            controller.Rebuild();
        }
    }
}
