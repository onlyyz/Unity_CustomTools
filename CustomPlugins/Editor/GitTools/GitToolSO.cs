using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


  [CreateAssetMenu(fileName = "GitToolSet",menuName = "GitTool/GitToolSo",order = 0)]
public class GitToolSO : SerializedScriptableObject
{
    // Start is called before the first frame update
     [SerializeField]
    public List<GitToolOne> listSet = new List<GitToolOne>();
}

  [Serializable]
public class GitToolOne {
      [SerializeField]

    [LabelText("操作人員")]
    public string zhiye  = null;

    [LabelText("文件列表")]
    [FilePath(AbsolutePath =false)]
    public List<string> FileAbsolutePath;

    [LabelText("目录名")]
    [FolderPath(AbsolutePath =false)]
    public List<string> AbsolutePath;
}