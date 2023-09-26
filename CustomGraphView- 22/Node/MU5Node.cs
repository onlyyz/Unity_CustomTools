using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace MU5Editor.NodeEditor
{
    public class MU5Node : Node
    {
        public string uid = string.Empty;
        public Dictionary<string, Port> port_dict;

        public Label uidLabel = new Label();

        //<Methods>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public void Init_UID()
        {
            if (uid != string.Empty) return;

            // uid = MU5Utility.GenerateUID();
            uid = Guid.NewGuid().ToString();
            viewDataKey = uid;
        }

        public void LoadData(NodeData nodeData)
        {
            uid = nodeData.uid;
            SetPosition(nodeData.localBound);
        }
    }
}