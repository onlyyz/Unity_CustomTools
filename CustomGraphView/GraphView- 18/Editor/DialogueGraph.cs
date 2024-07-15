using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Graph";
    
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

        GenerateMiniMap();
        
        GenerateBlackBoard();
    }


    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name");
        //设置为新的描述
        fileNameTextField.SetValueWithoutNotify(_fileName);
        //标记为脏，可改变一个词语，发生更改的时候，重新Drew，否则文本字段不会使用新的值
        fileNameTextField.MarkDirtyRepaint(); 
        //回调函数，以此更改文件名
        //变量从编辑器中获取文件名，随后将其添加
        fileNameTextField.RegisterValueChangedCallback(evt =>_fileName = evt.newValue);
        toolbar.Add(fileNameTextField);
            
            
        toolbar.Add(new Button( ()=>RequestDataOperation(true)){text = "Save Data"});
        toolbar.Add(new Button( ()=>RequestDataOperation(false)){text = "Load Data"}); 
        
        
        //Create Button
        // var nodeCreateButton = new Button(() => {_graphView.CreateNode("Dialogue Node");});
        // nodeCreateButton.text = "Create Node";
        // toolbar.Add(nodeCreateButton);
        
        
        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        //检查文件名是否为空
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file Name!", " Please Enter a valid file name","OK");
            return;
        }

        //GraphSaveUtility
        var SaveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            SaveUtility.SaveGraph(_fileName);
        }
        else
        {
            SaveUtility.LoadGraph(_fileName);
        }
    }


    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
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

    private void GenerateMiniMap()
    {
        
        var minimap = new MiniMap
        {
            anchored = true,
        };
        
        //This will give 10 px offset form left side
        var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2( this.maxSize.x - 10, 30));
        minimap.SetPosition(new Rect(cords.x,cords.y,200,140));
        // minimap.SetPosition(new Rect(10,30,200,140));
        _graphView.Add(minimap);
    }

    private void GenerateBlackBoard()
    {
        var blackBorad = new Blackboard(_graphView);
        blackBorad.Add(new BlackboardSection
        {
            title = "Exposed Properties"
        });
        //Add 
        blackBorad.addItemRequested = _blackBorad =>
        {
            _graphView.AddPropertyToBlackBoard(new ExposeProperty());
        };
        blackBorad.SetPosition(new Rect(10,30,200,300));
        //Blackboard, VisualElement, string
        blackBorad.editTextRequested = (blackboard, element, NewValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposeProperties.Any(x => x.PropertyName == NewValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one!",
                    "ok");
                return;
            }

            var propertyIndex = _graphView.ExposeProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            _graphView.ExposeProperties[propertyIndex].PropertyName = NewValue;
            ((BlackboardField)element).text = NewValue;
        };
        
        _graphView.Add(blackBorad);
        _graphView.Blackboard = blackBorad;
    }

   
}