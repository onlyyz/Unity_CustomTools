using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


public class SampleGraphView : GraphView
{
    public RootNode root;
    
    //构造函数中添加
    public SampleGraphView() : base()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        
        //背景色
        Insert(0, new GridBackground());
        
        //根節點
        root = new RootNode();
        AddElement(root);
        
        //移动控制器
        this.AddManipulator(new SelectionDragger());
        
        var searchWindowProvider = new SampleSearchWindowProvider();
        searchWindowProvider.Initialize(this);
        
        nodeCreationRequest += context =>
        {
            // AddElement(new SampleNode());
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);

        };
    }   
    
    // 节点适配器。
    public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
    {
        //兼容端口
        var compatiblePorts = new List<Port>();
        //不要连接到同一节点
        //输入到输入和输出到输出不连接。
        //如果端口中设置的类型不匹配，则不会连接。
        foreach (var port in ports.ToList())
        {
            if (startAnchor.node == port.node ||
                startAnchor.direction == port.direction ||
                startAnchor.portType != port.portType)
            {
                continue;
            }

            compatiblePorts.Add(port);
        }


        return compatiblePorts;
    }
    
    public void Execute()
    {
        var rootEdge = root.OutputPort.connections.FirstOrDefault();
        if (rootEdge == null) return;

        var currentNode = rootEdge.input.node as ProcessNode;

        while (true)
        {
            currentNode.Execute();

            var edge = currentNode.OutputPort.connections.FirstOrDefault();
            if (edge == null) break;

            currentNode = edge.input.node as ProcessNode;
        }
    }
}
   