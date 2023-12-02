using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


  [CreateAssetMenu(fileName = "LoadTestSO",menuName = "TestTool/LoadTestSO",order = 0)]
public class LoadTestSO : SerializedScriptableObject
{
    // Start is called before the first frame update
     [SerializeField]
    public List<LoadTestOne> listSet = new List<LoadTestOne>();
}

  [Serializable]
public class LoadTestOne {
      [SerializeField]

    [LabelText("加载房间备注")]
    public string roomName  = null;

    [LabelText("场景名SceneName")]
    public string SceneName;

    [LabelText("起始位置")]
    public Vector3 pos;
}