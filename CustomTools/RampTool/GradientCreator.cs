using CustomPlugins.Gradients;
using UnityEngine;
using UnityEditor;



public class GradientCreator : GradientCreatorEditor
{
    [MenuItem("Tools/GradientCreator")]
    private static void ShowWindow()
    {
        var window = GetWindow<GradientCreator>();
        window.titleContent = new GUIContent("创建：Gradient");
        window.Show();
    }
    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
        _assetLstProperty = _serializedObject.FindProperty(GradientData.Gradient);
    }
}