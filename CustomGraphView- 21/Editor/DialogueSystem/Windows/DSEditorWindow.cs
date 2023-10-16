using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "Dialogue File";
        private Button saveButton;
        [MenuItem("Tool/21-ScriptGraph")]
        public static void ShowExample()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }


        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            
            AddStyles();
        }




        #region Elements Addtion

        private void AddGraphView()
        {
            DSGraphView GraphView = new DSGraphView(this);
            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            //the TextField include the Call Back Function
            TextField fileNameTextField = DSElementUtility.CreateTextField(defaultFileName,"File Name:");
            // fileNameTextField.style.flexDirection = FlexDirection.Column;
            saveButton = DSElementUtility.CreateButton("Save");
            
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            
            
            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");
            
            rootVisualElement.Add(toolbar);
        }
        
        #endregion

        
        
        
        #region Styles
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }
        #endregion

        #region Utility Methods

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }
        
        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }

        #endregion
    }
}