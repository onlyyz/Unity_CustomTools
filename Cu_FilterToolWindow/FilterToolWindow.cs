using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FilterToolWindow : EditorWindow
{
    private GameObject[] allObjects;
    private string nameFilter = "";
    private string tagFilter = "Untagged"; // 默认标签为"Untagged"
    private int layerFilter = 0; // 默认选择 Default 层级
    private Material materialFilter;
    private BatchAction batchAction = BatchAction.None;
    private GameObject selectedObject; // 选中的物体

    // 新增公共变量以指定材质、标签和层级
    public Material newMaterial;
    public string newTag = "NewTag"; // 确保这个字段在你的类中已经声明，并具有默认值
    public int newLayer = 0; // 默认选择 Default 层级

    // 是否折叠数组
    private bool foldoutArray = false;

    // 批量操作的枚举
    private enum BatchAction
    {
        None,
        ChangeTag,
        ChangeLayer,
        ChangeMaterial
    }

    [MenuItem("Tool/Filter Tool Window")]
    public static void ShowWindow()
    {
        GetWindow<FilterToolWindow>("Filter Tool");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Filter Options", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 调整 "Name" 输入框与标签的距离
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name:", GUILayout.Width(50));
        nameFilter = EditorGUILayout.TextField(nameFilter);
        EditorGUILayout.EndHorizontal();

        // 使用 SerializedObject 和 SerializedProperty 处理标签
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tag:", GUILayout.Width(50));
        EditorGUI.BeginChangeCheck();
        tagFilter = EditorGUILayout.TagField(tagFilter);
        if (EditorGUI.EndChangeCheck())
        {
            // 如果标签改变，刷新显示
            GetAllObjectsInScenes();
        }

        // 使用 LayerField 选择层级
        EditorGUILayout.LabelField("Layer:", GUILayout.Width(50));
        layerFilter = EditorGUILayout.LayerField(layerFilter);
        EditorGUILayout.EndHorizontal();

        // 调整 "Material" 输入框与标签之间的距离
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Material:", GUILayout.Width(50));
        materialFilter = (Material)EditorGUILayout.ObjectField(materialFilter, typeof(Material), false);
        EditorGUILayout.EndHorizontal();

        // 过滤按钮
        if (GUILayout.Button("Refresh"))
        {
            GetAllObjectsInScenes();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Batch Actions", EditorStyles.boldLabel);

        // 使用水平布局包裹 "Action" 和执行按钮
        EditorGUILayout.BeginHorizontal("Box");
        // 调整 "Action" 输入框与标签之间的距离和大小
        EditorGUILayout.LabelField("Action:", GUILayout.Width(50));
        batchAction = (BatchAction)EditorGUILayout.EnumPopup(batchAction, GUILayout.ExpandWidth(true));
        switch (batchAction)
        {
            case BatchAction.ChangeTag:
                EditorGUILayout.LabelField("New Tag:", GUILayout.Width(60));
                // 使用列表选择标签
                string[] tagOptions = UnityEditorInternal.InternalEditorUtility.tags;
                int selectedIndex = System.Array.IndexOf(tagOptions, newTag);
                selectedIndex = EditorGUILayout.Popup(selectedIndex, tagOptions);
                newTag = tagOptions[Mathf.Clamp(selectedIndex, 0, tagOptions.Length - 1)];
                break;

            case BatchAction.ChangeLayer:
                EditorGUILayout.LabelField("New Layer:", GUILayout.Width(60));
                newLayer = EditorGUILayout.LayerField(newLayer);
                break;

            case BatchAction.ChangeMaterial:
                EditorGUILayout.LabelField("New Material:", GUILayout.Width(60));
                newMaterial = (Material)EditorGUILayout.ObjectField(newMaterial, typeof(Material), false);
                break;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 执行按钮
        if (GUILayout.Button("Execute"))
        {
            ExecuteBatchAction();
        }

        EditorGUILayout.Space();

        // 折叠数组
        foldoutArray = EditorGUILayout.Foldout(foldoutArray, "Filtered Objects", true);
        if (foldoutArray)
        {
            if (allObjects != null)
            {
                foreach (var obj in allObjects)
                {
                    // 在显示物体前检查是否为 null
                    if (obj != null && PassesFilter(obj))
                    {
                        EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    }
                }
            }
        }
    }

    private void GetAllObjectsInScenes()
    {
        Scene[] scenes = EditorSceneManager.GetAllScenes();
        List<GameObject> objectsList = new List<GameObject>();

        foreach (var scene in scenes)
        {
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
            GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in objectsInScene)
            {
                if (PassesFilter(obj))
                {
                    objectsList.Add(obj);
                }
            }
        }

        allObjects = objectsList.ToArray();
    }

    private bool PassesFilter(GameObject obj)
    {
        // 将原来的精确匹配修改为包含条件字符即可加入数组
        if (!string.IsNullOrEmpty(nameFilter) && !obj.name.ToLower().Contains(nameFilter.ToLower()))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(tagFilter) && obj.tag != tagFilter)
        {
            return false;
        }

        if (layerFilter != obj.layer)
        {
            return false;
        }

        if (materialFilter != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial != materialFilter)
            {
                return false;
            }
        }

        return true;
    }

    private void ExecuteBatchAction()
    {
        foreach (var obj in allObjects)
        {
            if (obj != null)
            {
                switch (batchAction)
                {
                    case BatchAction.ChangeTag:
                        obj.tag = newTag;
                        break;

                    case BatchAction.ChangeLayer:
                        obj.layer = newLayer;
                        break;

                    case BatchAction.ChangeMaterial:
                        Renderer renderer = obj.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.sharedMaterial = newMaterial;
                        }
                        break;
                }
            }
        }
    }
}
