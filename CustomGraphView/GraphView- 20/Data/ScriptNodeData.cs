using System;
using TreeEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptNodeData
{
    public int id;

    public TreeEditorHelper.NodeType type;

    public Rect rect;

    public int[] outIds;

    public byte[] serialData;
}