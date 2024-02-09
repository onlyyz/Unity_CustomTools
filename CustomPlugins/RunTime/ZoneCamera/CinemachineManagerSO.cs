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
        [SerializeField, LabelText("相机参数")] public CinemachineManager.CameraParameter m_CameraParameter;
        [SerializeField, LabelText("曲线覆盖")] public CinemachineBlendDefinition m_Blend;
        [SerializeField, LabelText("相机锁定")] public CinemachineManager.CameraLock CamLock;
        
    }
}