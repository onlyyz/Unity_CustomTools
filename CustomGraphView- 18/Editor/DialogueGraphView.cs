using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(250,200);
    
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
    
    //生成 初始 入口Node
    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode()
        {
                title = "start",
                GUID = Guid.NewGuid().ToString(),
                DialogueText = "ENTRYPOINT",
                EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);
        
        //入口不可删除,不可移动
        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;
        
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
        dialogueNode.inputContainer.Add(inputPort);

        //输出port的数量
        var button = new Button(() => {  AddchoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button); 
        
        
        //文本框
        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);
        
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
        
        
        //刷新节点
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        
        
        return dialogueNode;
    }

    public void AddchoicePort(DialogueNode dialogueNode, string overriddenPortName ="") 
    {
        //port 
        // var generatedPort = GeneratePort(dialogueNode, Direction.Output,Port.Capacity.Multi);
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);
        //Delete Port Test
        // var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        // generatedPort.contentContainer.Remove(oldLabel);
        
        
        //搜索端口名称指定为选择端口计数
        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        generatedPort.portName = $"Choice{outputPortCount}";

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"choice {outputPortCount}"
            :overriddenPortName;

      
        //添加文本
        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        //空隙
      
        //TODO:Bug 文本框有问题  
        // generatedPort.contentContainer.Add(textField);
        
        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "Θ"
        };
        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = choicePortName;
        
        
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }


    private void RemovePort(DialogueNode dialogueNode,Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName
                                                   && x.output.node == generatedPort.node);
        //如果不包含任何元素，退出
        if (targetEdge.Any())
        {
            //序列中的第一个元素
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }
       
        
        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
