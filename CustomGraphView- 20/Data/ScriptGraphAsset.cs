using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "scriptgraph.asset", menuName ="ScriptGraph Asset")]
public class ScriptGraphAsset : ScriptableObject
{
    public List<ScriptNodeData> list = new List<ScriptNodeData>();
}