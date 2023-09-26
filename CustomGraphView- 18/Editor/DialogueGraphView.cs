using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(150,200);
    
    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        
        
        //  放大/缩小。
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());


        //网格背景实例，并且作为背景插入，并且改变 背景颜色
        var grid = new GridBackground();
        Insert(0, grid);
        
        //  读取 uss 文件并将其添加到样式中
        // this.styleSheets.Add(Resources.Load<StyleSheet>("20-GraphView/Uss/GraphViewBackGround"));
        //  在图层最底层添加背景
        // this.Insert(0, new GridBackground());
        
        //添加到Graph View 基类
        AddElement(GenerateEntryPointNode());
    }

    //节点连接兼容问题，获取与给定端口兼容的所有端口
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //兼容端口
        var compatiblePorts = new List<Port>();
        
        ports.ForEach((port =>
        {
            if (startPort !=port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        }));
        return compatiblePorts;
    }


    //生成Port
    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        //不是在端口之间传递数据，并非是VFX shader，这里仅仅是检查是否有链接
        //Arbitrary type
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    
    //生成Node
    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode()
        {
                title = "start",
                GUID = Guid.NewGuid().ToString(),
                DialogueText = "ENTRYPOINT",
                // EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);
        //Refresh node Status
        node.RefreshExpandedState();
        node.RefreshPorts();
        
        node.SetPosition(new Rect(100,200,100,150));

        return node;
    }

    //添加节点到Graph view ，同时创建node
    public void CreateNode(String nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    
    
    //没有加入Graph view
    public DialogueNode CreateDialogueNode(String nodeName)
    {
        var dialogueNode = new DialogueNode()
        {
            title = nodeName,  
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        
        //Port the input and output
        var inputPort =  GeneratePort(dialogueNode, Direction.Input,Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.outputContainer.Add(inputPort);

        //输出port的数量
        var button = new Button(() => {  AddchoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button); 
        
        //刷新节点
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        
        
        return dialogueNode;
    }

    private void AddchoicePort(DialogueNode dialogueNode)
    {
        //port 
        var GeneratedPort = GeneratePort(dialogueNode, Direction.Output);
        
        //搜索端口名称指定为选择端口计数
        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        GeneratedPort.portName = $"choice {outputPortCount}";
        
        dialogueNode.outputContainer.Add(GeneratedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
