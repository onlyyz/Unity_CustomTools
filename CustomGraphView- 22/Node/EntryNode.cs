using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class EntryNode : MU5Node
    {
        //<Variables>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        Port outputPort;
        void Init_Port()
        {
            outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputPort.portName = "output";
            outputContainer.Add(outputPort);

            port_dict = new Dictionary<string, Port>(){
                { "500100",outputPort}
            };
        }

        //<Constructor>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public EntryNode() : base()
        {
            title = "Entry";
            titleContainer.style.backgroundColor = new StyleColor() { value = new Color(0, 0.6f, 0) };
            capabilities -= Capabilities.Deletable;
            Init_UID();
            Init_Port();
        }
    }
}