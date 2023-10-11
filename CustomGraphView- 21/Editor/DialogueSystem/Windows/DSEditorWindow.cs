using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Tool/21-ScriptGraph")]
        public static void ShowExample()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }


        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }




        #region Elements Addtion

        private void AddGraphView()
        {
            DSGraphView GraphView = new DSGraphView(this);
            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);
        }

        #endregion

        
        #region Styles
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }
        #endregion
    }
}