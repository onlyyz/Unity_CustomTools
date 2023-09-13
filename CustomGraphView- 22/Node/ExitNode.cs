using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class ExitNode : MU5Node
    {
        //<Variables>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        Port inputPort;
        void Init_Port()
        {
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "input";
            inputContainer.Add(inputPort);

            port_dict = new Dictionary<string, Port>(){
                {"000100",inputPort}
            };
        }

        //<Constructor>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public ExitNode() : base()
        {
            title = "Exit";
            titleContainer.style.backgroundColor = new StyleColor() { value = new Color(0.6f, 0, 0) };
            capabilities -= Capabilities.Deletable;
            Init_UID();
            Init_Port();
        }
    }
}