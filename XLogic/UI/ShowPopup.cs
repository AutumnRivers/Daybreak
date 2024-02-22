using System;
using System.Collections.Generic;
using System.Text;

using MPGraph;
using StoryGraph;

using UnityEngine;
using UnityEngine.UI;

namespace Daybreak_Midnight.XLogic.UI
{
    [XLogicGraphNode]
    public class XLogicShowPopup : FreeformXLogicNode
    {
        public static string ID = "ShowPopup";

        public override string NodeID => ID;
    }

    [XLogicInternalNode]
    public class ShowPopupNode : StoryGraphNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
        public TriggerPort remove;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, false)]
        public TriggerPort removed;

        public string note;

        public override string Name => "Show popup: " + note;

        public ShowPopupNode()
        {
            AddDynamicInput(typeof(TriggerPort), fieldName: "show");
            AddDynamicOutput(typeof(TriggerPort), fieldName: "showed");
        }

        public override void Trigger(bool synapse = false)
        {
            Popup.ShowMessage("", note);

            Next("showed", synapse);
        }
    }
}
