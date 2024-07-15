using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArcMove))]
public class ArcMoveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArcMove myScript = (ArcMove)target;
        if (GUILayout.Button("Start Move"))
        {
            myScript.StartMove();
        }
    }
}