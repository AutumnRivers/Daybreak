using System;
using System.Collections.Generic;
using System.Text;

using MPGraph;
using StoryGraph;

using UnityEngine;
using UnityEngine.UI;

namespace Daybreak_Midnight.XLogic.BlackMarket
{
    [XLogicNode]
    public class MPGraphAddSoftware : MPGraphNode<MPGraphNodeDataString>
    {
        public static string ID = "AddSoftwareNode";

        public Dropdown dropdown;

        public override string NodeID => ID;

        private readonly List<string> software =
        [
            "Ace", "Band1t", "Barricade", "Buffer", "B.uzzK1ll", "Cloak V2.0", "Cloak V3.3", "Copy", "Dagger", "Damocles",
            "Ech0", "Gatekeeper", "Harp00n", "Harp00n++", "HashRat", "Hive", "Jackhammer", "King", "Leech", "Mask", "Noise Bomb",
            "0ni0n", "Overclock", "Paint Brush", "Rainbow", "Shuriken", "Decoy", "Sniffer", "Sniffer 2.0", "Sniper", "Spider",
            "Spoon", "The Trashman", "Trojan", "Troll Toll", "Tunnel", "Vorpal Sword", "Pr0xy"
        ];

        public void ClearUnecessaryComponents()
        {
            var component = gameObject.GetComponent<MPGraphNodeMission>();

            DestroyImmediate(component);
        }

        public override void Init()
        {
            base.Init();
            RefreshDropdown();
        }

        private void RefreshDropdown()
        {
            dropdown.ClearOptions();

            List<Dropdown.OptionData> list = [];

            foreach(string program in software)
            {
                var data = new Dropdown.OptionData
                {
                    text = program
                };

                list.Add(data);
            }

            dropdown.AddOptions(list);
        }

        public override MPGraphNodeData Save()
        {
            if (dropdown.value < 0 || dropdown.value >= software.Count)
            {
                data.value = "";
            }
            else
            {
                data.value = software[dropdown.value];
            }

            return base.Save();
        }

        protected override void OnRestore(MPGraphNodeDataString data)
        {
            int index = software.FindIndex(s => s == data.value);

            dropdown.value = index > -1 ? index : 0;
        }
    }

    [XLogicInternalNode]
    public class AddSoftwareNode : StoryGraphNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
        public TriggerPort add;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, false)]
        public TriggerPort complete;

        public string programName;

        private ProgramInformation program;

        public override string Name => "Add Software: " + ((program != null) ? program.displayName : "NULL");

        public AddSoftwareNode()
        {
            AddDynamicInput(typeof(TriggerPort), fieldName: "add");
            AddDynamicOutput(typeof(TriggerPort), fieldName: "added");
        }

        public override void Trigger(bool synapse = false)
        {
            program = Resources.Load<ProgramInformation>("Data/ProgramInformation/" + programName);

            bool programExistsInBlackMarket = GameData.Instance.blackMarket.Exists(p => p == program);

            if(programExistsInBlackMarket)
            {
                base.Trigger(synapse);
                Next("added", synapse);
            } else if(synapse)
            {
                base.Trigger(synapse);
                if(!programExistsInBlackMarket)
                {
                    StoryControl.AddProgramToMarket(program);
                }

                Next("added", synapse);
            }
        }
    }
}
