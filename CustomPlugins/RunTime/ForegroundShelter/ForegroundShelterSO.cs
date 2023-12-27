using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPlugins.ForegroundShelter.SO
{
    public class ForegroundShelterSO : SerializedScriptableObject
    {
        [DictionaryDrawerSettings]
        public Dictionary<string, ForegroundShelterData> DictNearOcclusionData = new Dictionary<string, ForegroundShelterData>();
    }
    
    [TableList, TableColumnWidth(20, Resizable = false)]
    [Serializable]
    public class ForegroundShelterData : ScriptableObject
    {
        public enum TriggerMode
        {
            Single,
            Multiple
        }

        [SerializeField, LabelText("触发模式"), GUIColor(1, .0f, .0f, 1)]
        public ForegroundShelterData.TriggerMode triggerMode;

        [SerializeField, LabelText("目标透明度"), Range(0.0f, 1.0f)]
        public float TargetAlpha = 0.5f;

        [SerializeField, LabelText("平滑时间"), Range(0.0f, 1.0f)]
        public float LerpTime;

    }
}