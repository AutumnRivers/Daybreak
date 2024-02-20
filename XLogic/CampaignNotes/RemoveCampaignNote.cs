using System;
using System.Reflection;

using MPGraph;
using StoryGraph;

using UnityEngine;
using UnityEngine.UI;

namespace Daybreak_Midnight.XLogic.CampaignNotes
{
    [XLogicNode]
    public class MPGraphNodeRemoveCampaignNote : MPGraphNode<MPGraphNodeDataString>
    {
        public static string ID = "RemoveCampaignNote";

        public InputField inputField;

        public override string NodeID => ID;

        public override MPGraphNodeData Save()
        {
            data.value = inputField.text;
            return base.Save();
        }

        protected override void OnRestore(MPGraphNodeDataString data)
        {
            inputField.text = data.value;
        }
    }

    [XLogicInternalNode]
    public class RemoveCampaignNoteNode : StoryGraphNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
        public TriggerPort remove;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, false)]
        public TriggerPort removed;

        public string note;

        public override string Name => "Remove campaign note: " + note;

        public RemoveCampaignNoteNode()
        {
            AddDynamicInput(typeof(TriggerPort), fieldName: "remove");
            AddDynamicOutput(typeof(TriggerPort), fieldName: "removed");
        }

        public override void Trigger(bool synapse = false)
        {
            if (!GameData.Progress.campaignNotes.Contains(note))
            {
                base.Trigger(synapse);
                Next("removed", synapse);
            }
            else if (synapse)
            {
                base.Trigger(synapse);
                if (GameData.Progress.campaignNotes.Contains(note))
                {
                    GameData.Progress.campaignNotes.Remove(note);
                }

                Next("removed", synapse);
            }
        }
    }
}
