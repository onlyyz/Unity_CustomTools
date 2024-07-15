using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


public class SampleGraphEditorWindow : EditorWindow
{
    [MenuItem("Tool/19-GraphView")]
    public static void Open()
    {
        GetWindow<SampleGraphEditorWindow>("19-GraphView");
    }
    
    //add the Node for can Visual List
    void OnEnable()
    {
        var graphView = new SampleGraphView()
        {
            //指向该元素样式对象的引用。
            style = { flexGrow = 1 }
        };
        //add 一个子节点 to
        rootVisualElement.Add(graphView);
        
        rootVisualElement.Add(new Button(graphView.Execute) { text = "Execute" });
    }
}