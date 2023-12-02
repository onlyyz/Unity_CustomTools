using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

using System.IO;
using System.Diagnostics;
using System.Linq;
using FilePath = Sirenix.OdinInspector.FilePathAttribute;

public class LoadTestTool : OdinEditorWindow
{
    // Start is called before the first frame update
    const string SO_Path = "Assets/CustomPlugins/Editor/LoadTest/LoadTestSO.asset";


    [MenuItem("Tools/角色加载场景工具", false, 24)]
    private static void ShowWindow()
    {
        var window = GetWindow<LoadTestTool>();
        window.titleContent = new GUIContent("角色加载场景工具");
        window.Show();
    }



    // [LabelText("文件列表")]
    // [FilePath(AbsolutePath =true)]
    // public List<string> FileAbsolutePath;

    // [LabelText("目录名")]
    // [FolderPath(AbsolutePath =true)]
    // public List<string> AbsolutePath;
 
    [Title("编辑加载节点")]
    [InlineEditor]
    public LoadTestSO gitset;

[OnInspectorInit]
    void CreateData(){
        gitset = AssetDatabase.LoadAssetAtPath<LoadTestSO>(SO_Path);
    }


    [Title("选择加载房间")]
    [ValueDropdown("GetAllSO", ExpandAllMenuItems = true)]
    [LabelText("选择当前加载点")]
    public LoadTestOne dropV3;

    private static IEnumerable GetAllSO()
    {
        var gso = AssetDatabase.LoadAssetAtPath<LoadTestSO>(SO_Path);
        return gso.listSet.Select(x => new ValueDropdownItem<LoadTestOne>(x.roomName, x));
    }


    [Button("加载到所需位置", ButtonSizes.Large, Style = ButtonStyle.Box),]
    public void btnLoadroom()
    {
        PlayerManager.Self.SetCtrl(false,null);
        CoreManager.Self.LoadSceneAndLoadPlayerInPoint( dropV3.SceneName,false, dropV3.pos);
        PlayerManager.Self.SetCtrl(true,null);
        UnityEngine.Debug.Log("完成 加载指令");
    }


     [Button("打印关卡全局数据", ButtonSizes.Large, Style = ButtonStyle.Box),]
     public void PrintVar(){
       var obj =  APManager.Self.LevelGlobalVar.GetAll();
       foreach(var o in obj){
          if(o.Value.TypeID.ToString() == "boolean"){
               UnityEngine.Debug.Log(string.Format(">>>>> {0} : {1}",o.Key,(bool)o.Value.Value));
          }else{
               UnityEngine.Debug.Log(string.Format(">>>>>not bool>>>> {0} : {1} : {2}",o.Key,o.Value.TypeID.ToString(),(double)o.Value.Value));
          }

       }
     }

}
