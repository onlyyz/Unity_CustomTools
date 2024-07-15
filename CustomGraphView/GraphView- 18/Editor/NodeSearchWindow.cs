using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class NodeSearchWindow : ScriptableObject,ISearchWindowProvider
{
    
    private DialogueGraphView _graphView;
    private EditorWindow _windowindow;
    private Texture2D _indentatioinIcon;

    public void Initialize(DialogueGraphView graphView, EditorWindow window)
    {
        this._graphView = graphView;
        this._windowindow = window;

        //Indentation hack for search window as a transparent icon
        _indentatioinIcon = new Texture2D(1, 1);
        _indentatioinIcon.SetPixel(0,0,new Color(0,0,0,0),0);
        //copy to GPU use
        _indentatioinIcon.Apply();
    }
    
    //持续显示elements
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
            
            //反射结构
            new SearchTreeEntry(new GUIContent("Dialogue Node",_indentatioinIcon))
            {
                //注入数据
                // userData = typeof(DialogueNode),level = 2
                userData = new DialogueNode(),level = 2
            },
            
            
            new SearchTreeGroupEntry(new GUIContent("Branch Node"), 1)
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        //位置换算
        var worldMousePosition = _windowindow.rootVisualElement.ChangeCoordinatesTo(
            _windowindow.rootVisualElement.parent,
            context.screenMousePosition - _windowindow.position.position);
         
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        
        switch (SearchTreeEntry.userData)
        {
            case DialogueNode dialogueNode:
                _graphView.CreateNode("Dialogue Node",localMousePosition);
                return true;
            default:
                return false;
        }
    }
    
    
    
}
 