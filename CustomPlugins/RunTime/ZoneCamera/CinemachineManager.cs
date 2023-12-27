using System;
using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CustomPlugins.Manager
{
    using Zone.Cinemachine;
    using Zone.SO;

    public class CinemachineManager : ManagerBase<CinemachineManager>
    {
      
#if UNITY_EDITOR
        #region TextButton
        [SerializeField, LabelText("Debug调试")] public bool IsDebug;
        [SerializeField, LabelText("相机锁定调试")] public bool IsLock;
        
        [Button("保存数据"), ButtonGroup]
        public void SaveSelfData()
        {
            if (CinemachineManagerData == null)
                CreateField();
            SaveData();
            Debug.Log("保存数据");
        }

        [Button("加载数据"), ButtonGroup]
        public void GetSelfData()
        {
            GetData();
            Debug.Log("加载数据");
        }

        [Button("启用限制"), ButtonGroup]
        public void ShowValue1()
        {
            SlowMotionBegin();
        }

        [Button("取消限制"), ButtonGroup]
        public void ShowValue2()
        {
            SlowMotionEnd();
        }
        [Button("Debug"), ButtonGroup]
        public void ShowValue3()
        {
            Debug.Log(ZoneCinemachine.gameObject.name);
        }
        #endregion
#endif
        #region Panel variable
        [SerializeField, LabelText("参数存档")] public CinemachineManagerSO CinemachineManagerData;
        [SerializeField, LabelText("相机参数")] public CameraParameter m_CameraParameter;
        [SerializeField, LabelText("曲线覆盖")] public CinemachineBlendDefinition m_Blend;

        public struct CameraParameter
        {
            [Space] [Space] [EnumToggleButtons] [SerializeField, LabelText("近_相机："),]
            public camMode m_nearCamMode;

            [SerializeField, Range(0.1f, 10f), LabelText("参数：")]
            public float camNearValue;

            [SerializeField, Range(0.5f, 10f), LabelText("阈值：")]
            public float nearThreshold;

            [Space] [Space] [EnumToggleButtons] [SerializeField, LabelText("中_相机：")]
            public camMode m_MiddleCamMode;

            [SerializeField, Range(0.1f, 10f), LabelText("参数：")]
            public float camMiddleValue;

            [Space] [Space] [EnumToggleButtons] [SerializeField, LabelText("远_相机：")]
            public camMode m_farCamMode;

            [SerializeField, Range(0.1f, 20f), LabelText("参数：")]
            public float camFarValue;

            [SerializeField, Range(5f, 100f), LabelText("阈值：")]
            public float distanceThreshold;

            [Space] [Space] [SerializeField, Range(0.5f, 2), LabelText("边境抖动 时间：")]
            public float zeroTime;

            [SerializeField, Range(0.01f, 1f), LabelText("边境抖动 距离阈值：")]
            public float closeThreshold;
        }

        public enum camMode
        {
            时间,
            速度
        }

        #endregion

        CinemachineVirtualCamera LastCamera;
        CinemachineVirtualCamera CurrentCamera;
        CinemachineBrain m_brain;

        #region Initialization

        private void Awake()
        {
            Self = this;
        }

        private void OnEnable()
        {
            Initialization();
        }

        private void Initialization()
        {
            if (m_brain == null)
                m_brain = gameObject.GetComponent<CinemachineBrain>();

            // CinemachineCore.GetBlendOverride = BlendOverrideHandler;

            m_Blend.m_Time = 1.5f;
            
                m_brain.m_DefaultBlend = m_Blend;
            initSLowMotion();

            if (CamLock.slowMotionGo == null)
            {
                CamLock.slowMotionGo = transform.parent.GetComponent<ZoneCinemachine>().gameObject;
            }
        }


        CinemachineBlendDefinition BlendOverrideHandler(
            ICinemachineCamera fromVcam, ICinemachineCamera toVcam,
            CinemachineBlendDefinition defaultBlend,
            MonoBehaviour owner)
        {
            // Make this decision depending on whatever internal criteria you have
            var customBlend = defaultBlend;
            customBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            return customBlend;
            
        }

        #endregion

        #region Calculated smoothing time

        //计算距离 时间

        public void SetVirtualCamera(CinemachineVirtualCamera cam)
        {
            LastCamera = CurrentCamera;
            CurrentCamera = cam;
            if (LastCamera == null)
                return;
            camSwitching();
        }

        private float MathDistance()
        {
            return (float)
            (
                math.abs(Math.Round(
                    Vector2.Distance(LastCamera.transform.position, CurrentCamera.transform.position)
                    , 2))
            );
        }

        private void camSwitching()
        {
            if (MathDistance() == 0 || MathDistance() <= m_CameraParameter.closeThreshold)
            {
                m_brain.m_DefaultBlend.m_Time = m_CameraParameter.zeroTime;
                Debug.Log("距离：" + MathDistance() + "  时间: " + m_CameraParameter.zeroTime);
                return;
            }

            if (MathDistance() <= m_CameraParameter.nearThreshold)
            {
                SetVirtualCameraSmoothStep("近 ", m_CameraParameter.camNearValue, m_CameraParameter.m_nearCamMode);
            }
            else if (m_CameraParameter.nearThreshold < MathDistance() &&
                     MathDistance() <= m_CameraParameter.distanceThreshold)
            {
                SetVirtualCameraSmoothStep("中 ", m_CameraParameter.camMiddleValue, m_CameraParameter.m_MiddleCamMode);
            }
            else if (m_CameraParameter.distanceThreshold < MathDistance())
            {
                SetVirtualCameraSmoothStep("远 ", m_CameraParameter.camFarValue, m_CameraParameter.m_farCamMode);
            }
        }

        private void SetVirtualCameraSmoothStep(string distance, float setValue, camMode mode)
        {
            if (mode.ToString().Equals("时间"))
            {
                m_brain.m_DefaultBlend.m_Time = setValue;
#if UNITY_EDITOR
                if (!IsDebug) return;
                MyDebug(distance, mode.ToString(), setValue);
#endif
            }
            else
            {
                if (setValue == 0) return;
                m_brain.m_DefaultBlend.m_Time = MathDistance() / setValue;
#if UNITY_EDITOR
                if (!IsDebug) return;
                MyDebug(distance, mode.ToString(), setValue);
#endif
            }
        }

#if UNITY_EDITOR
        private void MyDebug(string distance, string mode, float value)
        {
            Debug.Log(distance + "距离：" + MathDistance() + "  " + mode + ": " + m_brain.m_DefaultBlend.m_Time);
        }
#endif
        #endregion

        #region Camera Lock

        [SerializeField, LabelText("相机锁定")] 
        private CameraLock CamLock;

        private ZoneCinemachine SlowCinemachine;
        private ZoneCinemachine ZoneCinemachine;
        private ZoneCinemachine LastZoneCinemachine;
        [HideInInspector] public bool inSlowMotion;
        [HideInInspector] public bool CanSlowMotion = true;
        [HideInInspector] public bool ExitSlowMotion = false;
        [HideInInspector] public int InZone;
        
       
        
        public struct CameraLock
        {
            [LabelText("相机锁定退出距离阈值")]
            [Range(3.0f, 12.0f)] public float SlowMitonThresholdDistance;
            public GameObject slowMotionGo;
        }
        
        private void initSLowMotion()
        {
            CanSlowMotion = true;
            inSlowMotion = false;
            ExitSlowMotion = false;
        }

        public void RegisterGo(GameObject ZonCam)
        {
            //121
            if (ZoneCinemachine != null)
            {
                LastZoneCinemachine = ZoneCinemachine;
            }
            //101
            SetBlendEnter();
          
            
            ZoneCinemachine = ZonCam.GetComponent<ZoneCinemachine>();
            SlowCinemachine = CamLock.slowMotionGo.GetComponent<ZoneCinemachine>();
        }
        
        // 121
        public void GoSwarp()
        {
            //101
            --InZone;
            
            if(LastZoneCinemachine == null) return;
            if (ZoneCinemachine.VirtualCamera.gameObject.activeInHierarchy) return;
            if (LastZoneCinemachine.VirtualCamera.gameObject.activeInHierarchy)
            {
                ZoneCinemachine = LastZoneCinemachine;
            }
        }
     
        //101
        private void SetBlendEnter()
        {
            var style = (++InZone == 1 && ExitSlowMotion);

            m_brain.m_DefaultBlend.m_Style =
                style ? CinemachineBlendDefinition.Style.Cut : CinemachineBlendDefinition.Style.EaseInOut;

            ExitSlowMotion = false;
        }
        
        public void SlowMotionBegin()
        {
            SlowMotionBeginSet();
        }

        private void SlowMotionBeginSet()
        {
            //锁定区域，进入其他触发区域，保留限制条件2
            if (!CanSlowMotion) return;
            if (inSlowMotion) return;
            CanSlowMotion = false;
            inSlowMotion = true;
            //101
            ExitSlowMotion = true;
            
#if UNITY_EDITOR
            if(IsLock) Debug.Log("镜头锁定： ");
#endif
            CamLock.slowMotionGo.transform.position = Self.transform.position;

            SetBlendTime();
            // SetSlowCameraPos(true);
            SlowCinemachine.ZoneCamEnter();
            ZoneCinemachine.ZoneCamExit();
        }

        
        
        public void SlowMotionEnd()
        {
            if (!inSlowMotion) return;
            m_brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            SetBlendTime();
            
#if UNITY_EDITOR
            if(IsLock) Debug.Log("镜头解锁： ");
#endif
            
         
            
            //121
            if(LastZoneCinemachine != null)
            {
                LastZoneCinemachine.SlowMotionEnter();
            }
            ZoneCinemachine.SlowMotionEnter();
           
            //101 Lerp And Only
          
            SlowCinemachine.ZoneCamExit();
            // StartCoroutine(SlowMotionExitLerpCurrenCamera());
            
            
            
            Invoke("SlowMotionFlag", 1.75f);
            inSlowMotion = false;
            
           
        }
        
        
        private void SlowMotionFlag()
        {
            CanSlowMotion = true;
        }
        
        // private void Update()
        // {
        //     Debug.Log(m_brain.ActiveBlend);
        //     Debug.Log(m_brain.IsBlending);
        // }

        //101 Only
        IEnumerator SlowMotionExitLerpCurrenCamera()
        {
            while (m_brain.IsBlending)
            {
                yield return null;
            }
            m_brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        }
        
        
        private void SetBlendTime()
        {
            m_brain.m_DefaultBlend.m_Time = 1.5f;
        }

        public bool GetIsSlowMotion()
        {
            return inSlowMotion;
        }

        public float GetCameraDistanceThea()
        {
            return CamLock.SlowMitonThresholdDistance;
        }
        
        #endregion

#if UNITY_EDITOR
        #region Editor

        private void SaveData()
        {
            CinemachineManagerData.m_nearCamMode = m_CameraParameter.m_nearCamMode;
            CinemachineManagerData.camNearValue = m_CameraParameter.camNearValue;
            CinemachineManagerData.nearThreshold = m_CameraParameter.nearThreshold;
            //
            CinemachineManagerData.m_MiddleCamMode = m_CameraParameter.m_MiddleCamMode;
            CinemachineManagerData.camMiddleValue = m_CameraParameter.camMiddleValue;
            //
            CinemachineManagerData.m_farCamMode = m_CameraParameter.m_farCamMode;
            CinemachineManagerData.camFarValue = m_CameraParameter.camFarValue;
            CinemachineManagerData.distanceThreshold = m_CameraParameter.distanceThreshold;
            //
            CinemachineManagerData.m_Blend = m_Blend;
            //
            CinemachineManagerData.zeroTime = m_CameraParameter.zeroTime;
            CinemachineManagerData.closeThreshold = m_CameraParameter.closeThreshold;
        }

        private void GetData()
        {
            m_CameraParameter.m_nearCamMode = CinemachineManagerData.m_nearCamMode;
            m_CameraParameter.camNearValue = CinemachineManagerData.camNearValue;
            m_CameraParameter.nearThreshold = CinemachineManagerData.nearThreshold;
            //
            m_CameraParameter.m_MiddleCamMode = CinemachineManagerData.m_MiddleCamMode;
            m_CameraParameter.camMiddleValue = CinemachineManagerData.camMiddleValue;
            //
            m_CameraParameter.m_farCamMode = CinemachineManagerData.m_farCamMode;
            m_CameraParameter.camFarValue = CinemachineManagerData.camFarValue;
            m_CameraParameter.distanceThreshold = CinemachineManagerData.distanceThreshold;
            //
            m_Blend = CinemachineManagerData.m_Blend;
            //
            m_CameraParameter.zeroTime = CinemachineManagerData.zeroTime;
            m_CameraParameter.closeThreshold = CinemachineManagerData.closeThreshold;
        }

        private void CreateField()
        {
            var ManagerData = ScriptableObject.CreateInstance<CinemachineManagerSO>();

            if (!AssetDatabase.IsValidFolder("Assets/CustomData/Resources"))
                AssetDatabase.CreateFolder("Assets/CustomData/Resources", "CinemachineManager");

            AssetDatabase.CreateAsset(ManagerData, $"Assets/CustomData/Editor/Resources/{"CinemachineManager"}.asset");
            AssetDatabase.SaveAssets();
            CinemachineManagerData = ManagerData;
            // if(isDebug) Debug.Log("创建文件：" +gameObject.scene.name);
        }

        #endregion
#endif
    }
}