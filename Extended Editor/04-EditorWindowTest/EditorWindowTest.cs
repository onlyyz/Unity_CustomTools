using UnityEngine;
using UnityEditor;

namespace EditorTutorial
{
    public class EditorWindowTest : EditorWindow 
    {

        [MenuItem("CustomEditorTutorial/WindowTest")]
        private static void ShowWindow() 
        {
            var window = GetWindow<EditorWindowTest>();
            window.titleContent = new GUIContent("WindowTest");
            window.Show();
        }

        private void OnGUI() 
        {
            if(GUILayout.Button("Click Me"))
            {
                //Logic
            }
        }
        
        private void OnFocus() 
        {
            //在2019版本是这个回调
            //订阅此事件，以便在场景视图调用 OnGUI 方法时接收回调。
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;

            //以前版本回调
            // SceneView.onSceneGUIDelegate -= OnSceneGUI
            // SceneView.onSceneGUIDelegate += OnSceneGUI
        }

        private void OnDestroy() 
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
        private void OnSceneGUI( SceneView view ) 
        {
        }
        
    }
}