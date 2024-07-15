using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using CustomPlugins.Gradients;
using Sirenix.OdinInspector;

public class GradientCreatorEditor : EditorWindow
{
  
    private int _GradientWidth = 128;
    private int _GradientHeight = 4;
    public GradientCreatorData _GradientData;
    private Texture2D _GradientMap;
  
    //Data
    [SerializeField,LabelText("行数与颜色")]
    protected List<Gradient> _Gradient = new List<Gradient>();
    protected SerializedObject _serializedObject;    
    protected SerializedProperty _assetLstProperty; 
    
    
    // Format
    string _GradientName;
    string[] MapFormat = {  "TGA", "PNG","JPG" };
    int FormatIndex = 0;
    


    
    private void OnGUI()
    {
        _GradientData = EditorGUILayout.ObjectField("Gradient 资源", _GradientData, typeof(GradientCreatorData), false) as GradientCreatorData;
        if (_GradientData)
        {
            _GradientWidth = _GradientData._GradientWidth;
        }
        else
        {
            _GradientWidth = 128;
        }
        GradientListGUI();
        _GradientMap = GradientLibrary.Create(_Gradient, _GradientWidth, _Gradient.Count * _GradientHeight);
        _GradientMap.wrapMode = TextureWrapMode.Clamp;
    
        //传递给Shader
        Shader.SetGlobalTexture(GradientData.Gradient, _GradientMap);
        SceneView.RepaintAll();
        Save();
    }

   
    private void GradientListGUI()//绘制列表
    {
        
        if (_GradientData)
        {
            _Gradient = _GradientData._Gradient;
        }
        
        
        _serializedObject.Update();
        //开始检查是否有修改
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_assetLstProperty, true);//显示属性 //第二个参数必须为true，否则无法显示子节点即List内容
        if (EditorGUI.EndChangeCheck())//结束检查是否有修改
        {
            _serializedObject.ApplyModifiedProperties();//提交修改
        }
    }
   
    private void Save()
    {
        
        if (_GradientData)
        {
            _GradientData._GradientWidth = EditorGUILayout.IntField("每条渐变宽度(像素)", _GradientWidth);
            _GradientHeight = _GradientData._GradientHeight;
            _GradientData._GradientHeight = EditorGUILayout.IntField("每条渐变高度(像素)", _GradientHeight);
        }
        else
        {
            _GradientWidth = EditorGUILayout.IntField("每条渐变宽度(像素)", _GradientWidth);
            _GradientHeight = EditorGUILayout.IntField("每条渐变高度(像素)", _GradientHeight);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("贴图名字", GUILayout.Width(100));
        if (_GradientData)
        {
            _GradientName = _GradientData._GradientName;
            _GradientData._GradientName = EditorGUILayout.TextArea(_GradientName);
        }
        else
        {
            _GradientName = EditorGUILayout.TextArea(_GradientName);
            
        }
        
        FormatIndex = EditorGUILayout.Popup(FormatIndex, MapFormat,GUILayout.Width(100));
        string _Format = ".tga";
        if (FormatIndex == 0)
            _Format = ".tga";
        if (FormatIndex == 1)
            _Format = ".tga";
        if (FormatIndex == 2)
            _Format = ".jpg";
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            string path = EditorUtility.SaveFolderPanel("Select an output path", "", "");

            if (_GradientData)
            {
                _GradientData._Gradient = this._Gradient;
            }

            byte[] pngData = _GradientMap.EncodeToPNG();
            File.WriteAllBytes(path + "/" + _GradientName + _Format, pngData);
            AssetDatabase.Refresh();
        }
    }
    
    
}