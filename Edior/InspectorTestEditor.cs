using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EditorTutorial
{

#if UNITY_EDITOR
    [CustomEditor(typeof(InspectorTest))]
    public class InspectorTestEditor : Editor 
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Click Me"))
            {

                InspectorTest ctr = target as InspectorTest;
                
                
                //Logic
                // serializedObject.FindProperty("Name").stringValue = "Codinggamer";
                // //引用属性修改
                // serializedObject.ApplyModifiedProperties();
                
                //记录使其可以撤销
                Undo.RecordObject( ctr ,"Change Name" );
                ctr.Name = "Codinggamer";
                EditorUtility.SetDirty( ctr );
            }
        }
    }
#endif
}
