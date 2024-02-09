using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropertyDrawerAction : PropertyDrawer
{
    protected enum ActionType
    {
        接受信号,
        发射信号,
        自定义
    }
        
    protected enum SignalType
    {
        单目标,
        多目标
    }
}
