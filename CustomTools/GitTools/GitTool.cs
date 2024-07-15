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

public class GitTool : OdinEditorWindow
{
    // Start is called before the first frame update
    const string SO_Path = "Assets/16_CustomPlugins/Editor/Tools/GitTools/GitToolSet.asset";


    [MenuItem("Tools/Git提交忽略目录", false, 24)]
    private static void ShowWindow()
    {
        var window = GetWindow<GitTool>();
        window.titleContent = new GUIContent("Git提交忽略目录");
        window.Show();
    }



    // [LabelText("文件列表")]
    // [FilePath(AbsolutePath =true)]
    // public List<string> FileAbsolutePath;

    // [LabelText("目录名")]
    // [FolderPath(AbsolutePath =true)]
    // public List<string> AbsolutePath;
 
    [Title("编辑类型文件目录")]
    [InlineEditor]
    public GitToolSO gitset;

[OnInspectorInit]
    void CreateData(){
        gitset = AssetDatabase.LoadAssetAtPath<GitToolSO>(SO_Path);
    }


    [Title("选择忽略职位类型")]
    [ValueDropdown("GetAllSO", ExpandAllMenuItems = true)]
    [LabelText("选择类型")]
    public List<GitToolOne> dropV3;

    private static IEnumerable GetAllSO()
    {
        var gso = AssetDatabase.LoadAssetAtPath<GitToolSO>(SO_Path);
        return gso.listSet.Select(x => new ValueDropdownItem<GitToolOne>(x.zhiye, x));
    }

    [Button("打印忽略目录文件", ButtonSizes.Large, Style = ButtonStyle.Box),]
    public void PrintSkip(){
          //Process.Start("git", cmdStr);
    }

    [Button("忽略所选类型目录文件", ButtonSizes.Large, Style = ButtonStyle.Box),]
    public void btnIgnore()
    {

        string projectPath = Path.GetDirectoryName(Application.dataPath);
        string cmdStr = "update-index --skip-worktree ";
        foreach (var one in dropV3)
        {
            var AbsolutePath = one.AbsolutePath;
            var FileAbsolutePath = one.FileAbsolutePath;

            foreach (string path in AbsolutePath)
            {
                UnityEngine.Debug.Log(path);
                var files = Directory.GetFiles(Path.Combine(projectPath, path));
                foreach (var file in files)
                {
                    UnityEngine.Debug.Log("git 临时忽略:" + file);
                    cmdStr += file.Replace("\\", "/") + " ";
                }


            }
            foreach (string filePah in FileAbsolutePath)
            {
                UnityEngine.Debug.Log("git 临时忽略:" + filePah);
                cmdStr += Path.Combine(projectPath, filePah) + " ";
            }

        }

        Process.Start("git", cmdStr);
        UnityEngine.Debug.Log("完成 git临时忽略指令！" + cmdStr);
    }

    [Button("取消忽略", ButtonSizes.Large, Style = ButtonStyle.Box),]
    public void btnIgnoreRetrun()
    {

        string projectPath = Path.GetDirectoryName(Application.dataPath);
        string cmdStr = "update-index --no-skip-worktree ";
        foreach (var one in dropV3)
        {
            var AbsolutePath = one.AbsolutePath;
            var FileAbsolutePath = one.FileAbsolutePath;

            foreach (string path in AbsolutePath)
            {
                UnityEngine.Debug.Log(path);
                var files = Directory.GetFiles(Path.Combine(projectPath, path));
                foreach (var file in files)
                {
                    UnityEngine.Debug.Log("git 临时忽略:" + file);
                    cmdStr += file.Replace("\\", "/") + " ";
                }
            }
            foreach (string filePah in FileAbsolutePath)
            {
                UnityEngine.Debug.Log("git 临时忽略:" + filePah);
                cmdStr += Path.Combine(projectPath, filePah) + " ";
            }
            Process.Start("git", cmdStr);
            UnityEngine.Debug.Log("完成 git取消忽略指令！");
        }

    }
}
