using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
   public List<NodeLinkData> Nodelinks = new List<NodeLinkData>();
   public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
}
