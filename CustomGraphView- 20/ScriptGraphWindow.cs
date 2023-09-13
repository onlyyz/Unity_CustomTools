using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptGraphWindow : EditorWindow
{
    [MenuItem("Tool/20-ScriptGraph")]
    public static void Open()
    {
        ScriptGraphWindow window = GetWindow<ScriptGraphWindow>();
        window.titleContent = new GUIContent("20-Dialogue Graph");
        window.Show();
    }
    
    private void OnEnable()
    {
        var scriptGraph = new ScriptGraphView(this);
        this.rootVisualElement.Add(scriptGraph);
    }
}
