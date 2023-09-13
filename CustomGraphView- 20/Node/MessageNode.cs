using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class MessageNode : ScriptGraphNode
{
    
    private TextField textField;

    public MessageNode()
    {
        // 给节点加标题。
        this.title = "Message";
  
        // 创建端口 Port （见下文）
        //用于创建端口的工厂方法。 定位
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
        inputPort.portName = "In";
        inputContainer.Add(inputPort);
        
        var outputOort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
        outputOort.portName = "Out";
        outputContainer.Add(outputOort);

        // 在节点的主要部分添加输入文本字段
        textField = new TextField();
        // 在节点的主要部分添加输入字段,允许多行
        textField.multiline = true;
        //IME输入法支持
        textField.RegisterCallback<FocusInEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.On; });
        textField.RegisterCallback<FocusOutEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });

        this.mainContainer.Add(textField);
    }
}