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
    public class XLogicAddSoftware : DropdownXLogicNode
    {
        public static string ID = "AddSoftwareNode";

        public override string NodeID => ID;

        private readonly List<string> software =
        [
            "Ace", "Band1t", "Barricade", "Buffer", "B.uzzK1ll", "Cloak V2.0", "Cloak V3.3", "Copy", "Dagger", "Damocles",
            "Ech0", "Gatekeeper", "Harp00n", "Harp00n++", "HashRat", "Hive", "Jackhammer", "King", "Leech", "Mask", "Noise Bomb",
            "0ni0n", "Overclock", "Paint Brush", "Rainbow", "Shuriken", "Decoy", "Sniffer", "Sniffer 2.0", "Sniper", "Spider",
            "Spoon", "The Trashman", "Trojan", "Troll Toll", "Tunnel", "Vorpal Sword", "Pr0xy"
        ];

        public XLogicAddSoftware()
        {
            DropdownItems = software;
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
