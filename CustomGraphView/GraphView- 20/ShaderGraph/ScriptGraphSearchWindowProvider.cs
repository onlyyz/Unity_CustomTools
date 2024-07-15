using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptGraphSearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private ScriptGraphWindow _window;
    private ScriptGraphView _graphView;
    private ScriptGraphAsset _scriptGraphAsset;

    public void Init(ScriptGraphView graphView,ScriptGraphWindow window)
    {
        _window = window;
        _graphView = graphView;
    }

    //搜索树条目，可以在Graph View 中显示，生成数据以填充搜索窗口，搜索窗口首次打开和重新加载时都会调用该方法
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var entries = new List<SearchTreeEntry>();
        
        //节点名字
        entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Custom Node")));
        /*
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(MessageNode)))
        {
            level = 1, userData = typeof(MessageNode)
        });
        */
        
        //	 当前域获得程序集
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            //程序集中获得类型
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass == false) continue;

                if (type.IsAbstract) continue;

                if (type.IsSubclassOf(typeof(ScriptGraphNode)) == false) continue;

                //userData 用户指定的对象，用于将应用程序特定数据附加到搜索树条目。
                entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
            }
        }
        
        return entries;
    }
    
    //在搜索树列表中选择一个条目，在搜索树列表中选择一个条目。
    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        //节点数据
        var type = SearchTreeEntry.userData as Type;
        //数据填充到实例
        var node = Activator.CreateInstance(type) as Node;
        
        // 创建节点的位置与鼠标坐标一致。 
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        
        node.SetPosition(new Rect(localMousePosition, new Vector2(00, 00)));
        
        _graphView.AddElement(node);
        
        
        
        //保存数据
        node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));

        _graphView.AddElement(node);

        // ScriptNodeData data = Serialize(node);

        //　让我们在这里补充一下！
        // _scriptGraphAsset.list.Add(data);
        return true;
    }
    
    
    
  
}