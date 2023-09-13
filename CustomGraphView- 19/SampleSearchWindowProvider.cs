using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class SampleSearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private SampleGraphView graphView;
    private SampleGraphEditorWindow graphWindow;

    public void Initialize(SampleGraphView graphView)
    {
        this.graphView = graphView;
     
    }

    //搜索树条目，可以在Graph View 中显示，生成数据以填充搜索窗口，搜索窗口首次打开和重新加载时都会调用该方法
    List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
    {
        var entries = new List<SearchTreeEntry>();
        entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));
        //	 当前域获得程序集
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            //程序集中获得类型
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(SampleNode)))
                    && type != typeof(RootNode))
                {
                    entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
                }
            }
        }

        return entries;
    }

    //在搜索树列表中选择一个条目，在搜索树列表中选择一个条目。
    bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        //节点数据
        var type = searchTreeEntry.userData as System.Type;
        //数据填充到实例
        var node = Activator.CreateInstance(type) as SampleNode;
        
        // 创建节点的位置与鼠标坐标一致。 
        // var worldMousePosition = graphWindow.rootVisualElement.ChangeCoordinatesTo(graphWindow.rootVisualElement.parent,context.screenMousePosition - graphWindow.position.position);
        // var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        //
        // node.SetPosition(new Rect(localMousePosition, new Vector2(00, 00)));
        
        graphView.AddElement(node);
        return true;
    }
}