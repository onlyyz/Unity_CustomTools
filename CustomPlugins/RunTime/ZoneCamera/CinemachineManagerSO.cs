using System;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPlugins.Zone.SO
{
    using Manager;
    [Serializable]
    public class CinemachineManagerSO : SerializedScriptableObject
    {
       
        [Space] [Space]  
        [EnumToggleButtons]
        
        [SerializeField,LabelText("近_相机："), ]
        public CinemachineManager.camMode m_nearCamMode;
        [SerializeField,Range(0.1f, 10f), LabelText("参数：")]
        public float camNearValue;
        [SerializeField,Range(0.5f, 10f), LabelText("阈值：")]
        public float nearThreshold;
        [Space] [Space]
        
        [EnumToggleButtons]
        [SerializeField,LabelText("中_相机：")]
        public CinemachineManager.camMode m_MiddleCamMode;
        [SerializeField,Range(0.1f, 10f), LabelText("参数：")] 
        public float camMiddleValue;
        [Space] [Space]
        
        
        [EnumToggleButtons]
        [SerializeField,LabelText("远_相机：")]
        public CinemachineManager.camMode m_farCamMode;
        [SerializeField,Range(0.1f, 20f), LabelText("参数：")]
        public float camFarValue;
        [SerializeField,Range(5f, 100f), LabelText("阈值：")]
        public float distanceThreshold;

        [SerializeField,LabelText("曲线覆盖")]
        public bool needToCustomizeBlend = true;
        public CinemachineBlendDefinition m_Blend;
        
        
        [SerializeField,Range(0.5f, 2), LabelText("边境抖动 时间：")]
        public float zeroTime = 0.5f;

        [SerializeField,Range(0.01f, 1f), LabelText("边境抖动 距离阈值：")]
        public float closeThreshold;
        
    }
}