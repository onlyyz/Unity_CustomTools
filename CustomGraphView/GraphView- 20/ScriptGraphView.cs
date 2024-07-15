using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ScriptGraphView : GraphView
{
    //  子窗口
    public ScriptGraphView(ScriptGraphWindow window) : base()
    {
        //  大小与父窗口大小一致
        this.StretchToParentSize();
        //  放大/缩小。
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        //  拖动移动绘图区域。
        this.AddManipulator(new ContentDragger());
        //  拖动移动选定的元素
        this.AddManipulator(new SelectionDragger());
        //  拖动选择范围
        this.AddManipulator(new RectangleSelector());
         
        //  读取 uss 文件并将其添加到样式中
        this.styleSheets.Add(Resources.Load<StyleSheet>("GraphViewBackGround"));

        //  在图层最底层添加背景
        this.Insert(0, new GridBackground());
        
        
        
        //  节点加入
        this.Add(new MessageNode());
        
        
        //  右侧页面
        var searchWindowProvider = ScriptableObject.CreateInstance<ScriptGraphSearchWindowProvider>();
        searchWindowProvider.Init(this, window);
        this.nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
        };
        
        
    }
    
    //获取与给定端口兼容的所有端口。连接规则
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //兼容端口
        var compatiblePorts = new List<Port>();
        foreach (var port in ports.ToList())
        {
            // 不能连接同一节点
            if (startPort.node == port.node) continue;
            // 不能连接输入 - 输入、输出 - 输出
            if (startPort.direction == port.direction) continue;
            // 不能连接不同的端口类型
            if (startPort.portType != port.portType) continue;
            compatiblePorts.Add(port);
        }
        return compatiblePorts;
    }
    
    
    
}