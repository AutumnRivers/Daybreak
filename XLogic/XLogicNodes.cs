using System;
using System.Collections.Generic;
using System.Text;
using Daybreak_Midnight.Helpers;
using MPGraph;

using UnityEngine;
using UnityEngine.UI;

namespace Daybreak_Midnight.XLogic
{
    public class FreeformXLogicNode : MPGraphNode<MPGraphNodeDataString>
    {
        public InputField inputField;

        public override string NodeID => throw new NotImplementedException();

        public void ClearUnecessaryComponents()
        {
            var component = gameObject.GetComponent<MPGraphNodeAddCampaignNote>();

            DestroyImmediate(component);
        }

        internal void SetPrivateField<T>(string fieldName, T newValue)
        {
            this.SetPrivateMPGraphNodeField(fieldName, newValue);
        }

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
}
