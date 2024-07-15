using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ReferenceFinder : EditorWindow
{
    private enum FilterType
    {
        Script,
        Shader,
        Texture,
        Material,
        Spine
    }

    private FilterType selectedFilterType;
    private MonoScript selectedScript;
    private Shader selectedShader;
    private Texture selectedTexture;
    private Material selectedMaterial;
    private Object selectedSpine;
    private Vector2 scrollPosition;
    private Dictionary<System.Type, List<Object>> referencingAssetsByType;
    private HashSet<System.Type> displayedTypes = new HashSet<System.Type>();
    private Dictionary<System.Type, bool> typeDisplayStatus = new Dictionary<System.Type, bool>();

    [MenuItem("Tools/引用查找")]
    public static void ShowWindow()
    {
        GetWindow<ReferenceFinder>("引用查找");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Filter Type", EditorStyles.boldLabel);
        selectedFilterType = (FilterType)EditorGUILayout.EnumPopup("Filter Type", selectedFilterType);

        switch (selectedFilterType)
        {
            case FilterType.Script:
                selectedScript = EditorGUILayout.ObjectField("Script", selectedScript, typeof(MonoScript), false) as MonoScript;
                selectedShader = null;
                selectedTexture = null;
                selectedMaterial = null;
                selectedSpine = null;
                break;
            case FilterType.Shader:
                selectedShader = EditorGUILayout.ObjectField("Shader", selectedShader, typeof(Shader), false) as Shader;
                selectedScript = null;
                selectedTexture = null;
                selectedMaterial = null;
                selectedSpine = null;
                break;
            case FilterType.Texture:
                selectedTexture = EditorGUILayout.ObjectField("Texture", selectedTexture, typeof(Texture), false) as Texture;
                selectedScript = null;
                selectedShader = null;
                selectedMaterial = null;
                selectedSpine = null;
                break;
            case FilterType.Material:
                selectedMaterial = EditorGUILayout.ObjectField("Material", selectedMaterial, typeof(Material), false) as Material;
                selectedScript = null;
                selectedShader = null;
                selectedTexture = null;
                selectedSpine = null;
                break;
            case FilterType.Spine:
                selectedSpine = EditorGUILayout.ObjectField("Spine", selectedSpine, typeof(Object), false) as Object;
                selectedScript = null;
                selectedShader = null;
                selectedTexture = null;
                selectedMaterial = null;
                break;
        }

        if ((selectedScript != null || selectedShader != null || selectedTexture != null || selectedMaterial != null || selectedSpine != null) && GUILayout.Button("Find References"))
        {
            FindReferences();
        }

        if (referencingAssetsByType != null && referencingAssetsByType.Count > 0)
        {
            GUILayout.Label("Referencing Assets:", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (var type in referencingAssetsByType.Keys)
            {
                bool isDisplayed = typeDisplayStatus.ContainsKey(type) && typeDisplayStatus[type];
                GUI.backgroundColor = isDisplayed ? Color.yellow : GUI.backgroundColor;

                if (GUILayout.Button(EditorGUIUtility.ObjectContent(null, type).image, GUILayout.Width(32), GUILayout.Height(32)))
                {
                    typeDisplayStatus[type] = !isDisplayed;
                }

                GUI.backgroundColor = Color.white;
            }

            foreach (var kvp in referencingAssetsByType)
            {
                if (typeDisplayStatus.ContainsKey(kvp.Key) && typeDisplayStatus[kvp.Key])
                {
                    GUILayout.Label(kvp.Key.Name, EditorStyles.boldLabel);

                    foreach (var asset in kvp.Value)
                    {
                        EditorGUILayout.BeginHorizontal();

                        // Get the icon of the asset
                        Texture assetIcon = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(asset));
                        GUILayout.Label(assetIcon, GUILayout.Width(20), GUILayout.Height(20));

                        // Display asset name and type
                        if (GUILayout.Button($"{asset.name} ({asset.GetType().Name})"))
                        {
                            Selection.activeObject = asset;
                            EditorGUIUtility.PingObject(asset);
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
        else if (referencingAssetsByType != null && referencingAssetsByType.Count == 0)
        {
            GUILayout.Label("没有引用该资源的其他资源。", EditorStyles.boldLabel);
        }
    }

    private void FindReferences()
    {
        string assetPath = null;
        if (selectedScript != null) assetPath = AssetDatabase.GetAssetPath(selectedScript);
        if (selectedShader != null) assetPath = AssetDatabase.GetAssetPath(selectedShader);
        if (selectedTexture != null) assetPath = AssetDatabase.GetAssetPath(selectedTexture);
        if (selectedMaterial != null) assetPath = AssetDatabase.GetAssetPath(selectedMaterial);
        if (selectedSpine != null) assetPath = AssetDatabase.GetAssetPath(selectedSpine);

        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        Dictionary<System.Type, List<Object>> dependenciesByType = new Dictionary<System.Type, List<Object>>();

        foreach (var path in allAssetPaths)
        {
            // Optimize dependency check by skipping irrelevant assets
            if (path.EndsWith(".cs") || path.EndsWith(".shader") || path.EndsWith(".meta"))
                continue;

            string[] assetDependencies = AssetDatabase.GetDependencies(path, true);
            foreach (var dependency in assetDependencies)
            {
                if (dependency == assetPath)
                {
                    Object asset = AssetDatabase.LoadMainAssetAtPath(path);
                    if (asset != null)
                    {
                        System.Type assetType = asset.GetType();
                        if (!dependenciesByType.ContainsKey(assetType))
                        {
                            dependenciesByType[assetType] = new List<Object>();
                        }
                        dependenciesByType[assetType].Add(asset);
                    }
                }
            }
        }

        referencingAssetsByType = dependenciesByType;
        displayedTypes.Clear();
        typeDisplayStatus.Clear();
    }
}
