using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    [MenuItem("Tool/18-Dialogue")]
    public static void OpenDialogueGraphWindow()
    {
        var Window = GetWindow<DialogueGraph>();
        Window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        //Tool Graph Virw
        ConstructGraphView();
        GenerateToolbar();
    }


    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        var nodeCreateButton = new Button(() =>
        {
            _graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        
        toolbar.Add(nodeCreateButton);
        rootVisualElement.Add(toolbar);
    }
    
    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };
      
        //Graph View填满窗口 
        _graphView.StretchToParentSize();
      
        rootVisualElement.Add(_graphView);

    }


    
    
    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }


   
}