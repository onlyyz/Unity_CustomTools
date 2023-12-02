using UnityEngine;
using UnityEditor;

public class GameObjectInstantiator : EditorWindow
{
    private GameObject objectToInstantiate;
    private GameObject referencePositionObject;
    private GameObject parentObject;
    private string parentObjectName = "ParentObject"; // 默认父物体名字
    private int numRows = 3;
    private int numColumns = 3;
    private float rowSpacing = 1.0f;
    private float columnSpacing = 1.0f;

    private bool useSpecifiedParent = false; // 新增字段，用于标记是否使用指定的父物体

    [MenuItem("Tools/YZ/实例化物体", false, 24)]
    public static void ShowWindow()
    {
        GetWindow<GameObjectInstantiator>("实例化物体");
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("要实例化的物体");
        objectToInstantiate = EditorGUILayout.ObjectField(objectToInstantiate, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("位置参考的物体");
        referencePositionObject = EditorGUILayout.ObjectField(referencePositionObject, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("生成父物体的物体");
        parentObject = EditorGUILayout.ObjectField(parentObject, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        // 新增字段的编辑器窗口控制
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("使用指定的父物体");
        useSpecifiedParent = EditorGUILayout.Toggle(useSpecifiedParent);
        EditorGUILayout.EndHorizontal();

        // 如果使用指定的父物体，则禁用父物体名字的编辑
        GUI.enabled = !useSpecifiedParent;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("父物体的名字");
        parentObjectName = EditorGUILayout.TextField(parentObjectName);
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("行数");
        numRows = EditorGUILayout.IntField(numRows);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("列数");
        numColumns = EditorGUILayout.IntField(numColumns);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("行间距");
        rowSpacing = EditorGUILayout.FloatField(rowSpacing);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("列间距");
        columnSpacing = EditorGUILayout.FloatField(columnSpacing);
        EditorGUILayout.EndHorizontal();

        // 在窗口中添加按钮，点击后执行实例化操作
        if (GUILayout.Button("实例化物体"))
        {
            // 在这里加入你的实例化逻辑
            // InstantiateGameObjects();
        }

        // 在窗口中添加按钮，点击后执行销毁操作
        if (GUILayout.Button("销毁实例化物体"))
        {
            // 在这里加入你的销毁逻辑
            // DestroyInstantiatedObjects();
        }
    }
}
