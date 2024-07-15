using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class SampleNode : MU5Node
    {
        //<Variables>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        Port inputPort1;
        Port inputPort2;
        Port outputPort1;
        Port outputPort2;
        Port outputPort3;

        void Init_Port()
        {
            inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort1.portName = "in1";
            inputContainer.Add(inputPort1);

            inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort2.portName = "in2";
            inputContainer.Add(inputPort2);

            outputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputPort1.portName = "out1";
            outputContainer.Add(outputPort1);

            outputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputPort2.portName = "out2";
            outputContainer.Add(outputPort2);

            outputPort3 = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputPort3.portName = "out3";
            outputContainer.Add(outputPort3);

            port_dict = new Dictionary<string, Port>(){
                {"000100",inputPort1},
                {"000200",inputPort2},
                {"500100",outputPort1},
                {"500200",outputPort2},
                {"500300",outputPort3}
            };
        }

        //<Constructor>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public SampleNode() : base()
        {
            title = "サンプル";
            Init_UID();
            Init_Port();
        }
    }
}
