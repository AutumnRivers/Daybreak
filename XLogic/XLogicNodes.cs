using System;
using System.Collections.Generic;
using System.Text;
using Daybreak_Midnight.Helpers;
using MPGraph;

using UnityEngine;
using UnityEngine.UI;

namespace Daybreak_Midnight.XLogic
{
    public class XLogicNode : MPGraphNode<MPGraphNodeDataString>
    {
        public override string NodeID => throw new NotImplementedException();

        public void AddSelected(GameObject selectedObject)
        {
            this.SetPrivateXLogicNodeField("selected", selectedObject);
        }
    }

    public class FreeformXLogicNode : XLogicNode
    {
        public InputField inputField;

        public void ClearUnecessaryComponents()
        {
            var component = gameObject.GetComponent<MPGraphNodeAddCampaignNote>();

            DestroyImmediate(component);
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

    public class DropdownXLogicNode : XLogicNode
    {
        public Dropdown dropdown;

        public List<string> DropdownItems { get; protected set; }

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

        protected void RefreshDropdown()
        {
            dropdown.ClearOptions();

            List<Dropdown.OptionData> list = [];

            if(DropdownItems == null || DropdownItems?.Count <= 0)
            {
                dropdown.AddOptions(["-- EMPTY --"]);
                return;
            }

            foreach (string item in DropdownItems)
            {
                var data = new Dropdown.OptionData
                {
                    text = item
                };

                list.Add(data);
            }

            dropdown.AddOptions(list);
        }

        public override MPGraphNodeData Save()
        {
            if (dropdown.value < 0 || dropdown.value >= DropdownItems.Count)
            {
                data.value = "";
            }
            else
            {
                data.value = DropdownItems[dropdown.value];
            }

            return base.Save();
        }

        protected override void OnRestore(MPGraphNodeDataString data)
        {
            int index = DropdownItems.FindIndex(s => s == data.value);

            dropdown.value = index > -1 ? index : 0;
        }
    }
}
