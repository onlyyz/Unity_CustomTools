using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Winndos
{
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




        #region Graph View

        private void AddGraphView()
        {
            DSGraphView GraphView = new DSGraphView();
            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);
        }

        #endregion
        
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}