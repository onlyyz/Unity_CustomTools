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
        [SerializeField, LabelText("是否有锁定相机")] public bool IsLockCamera;
#if UNITY_EDITOR

        #region TextButton
        [SerializeField, LabelText("Debug调试")] public bool IsDebug;
        [SerializeField, LabelText("Drew调试")] public bool IsDrew;

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
            CameraLockEnter();
            InCameraLockState = true;
        }

        [Button("取消限制"), ButtonGroup]
        public void ShowValue2()
        {
            CameraLockExit();
            InCameraLockState = false;
        }


        #endregion

#endif
        //========================================================================
        
        
        #region Panel variable
        [SerializeField, LabelText("参数存档")] public CinemachineManagerSO CinemachineManagerData;
        
        [SerializeField, LabelText("曲线覆盖")] public CinemachineBlendDefinition m_Blend;
        [SerializeField, LabelText("相机参数")] public CameraParameter m_CameraParameter;
      

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

        //========================================================================
        
        string currentSence, lastSecne;
        CinemachineBrain m_brain;
        CinemachineVirtualCamera LastCamera;
        CinemachineVirtualCamera CurrentCamera;

        //========================================================================

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

            CinemachineCore.GetBlendOverride = BlendOverrideHandler;

            m_brain.m_DefaultBlend = m_Blend;


            if (CamLock.LockGo == null)
            {
                Debug.Log("相机锁定部件丢失");
                CamLock.LockGo = transform.parent.GetComponent<ZoneCinemachine>().gameObject;
            }

            // m_brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            // m_Blend.m_Time = 1.5f;


            InitSmoothTime();
            InitLockCamera();
        }
        
        CinemachineBlendDefinition BlendOverrideHandler(
            ICinemachineCamera fromVcam, ICinemachineCamera toVcam,
            CinemachineBlendDefinition defaultBlend,
            MonoBehaviour owner)
        {
            // Make this decision depending on whatever internal criteria you have
            var customBlend = defaultBlend;
            customBlend.m_Style = m_Blend.m_Style;
            return customBlend;
        }

        #endregion

        #region Switch Scene

        private IEnumerator InvokeCameraMode()
        {
            yield return new WaitForEndOfFrame();
            m_brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        }

        #endregion

        //========================================================================


        #region Calculated smoothing time

        void InitSmoothTime()
        {
            currentSence = Self.gameObject.scene.name;
            lastSecne = currentSence;
        }
        
        //计算距离 时间
        //场景缓冲
        public void SetVirtualCamera(CinemachineVirtualCamera cam)
        {
            //切换场景 
            lastSecne = currentSence;
            currentSence = cam.gameObject.scene.name;

            m_Blend.m_Style = (currentSence.Equals(lastSecne))
                ? CinemachineBlendDefinition.Style.Custom
                : CinemachineBlendDefinition.Style.Cut;

                // m_brain.m_DefaultBlend.m_Style = m_Blend.m_Style;
                // m_brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
            

            // Debug.Log(lastSecne + "   " + currentSence );
            // Debug.Log("相机切换：" +  m_brain.m_DefaultBlend.m_Style + " " +  m_brain.m_DefaultBlend.m_Time);
            //计算速度
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
                // Debug.Log("距离：" + MathDistance() + "  时间: " + m_CameraParameter.zeroTime);
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


        //========================================================================


        #region Camera Lock

        [SerializeField, LabelText("相机锁定")] public CameraLock CamLock;
        public struct CameraLock
        {
            [LabelText("自动解锁距离阈值")] [Range(2.0f, 12.0f)]
            public float SlowMitonThresholdDistance;

            [LabelText("自动解锁等待时间")] [Range(2.0f, 5.0f)]
            public float cameraWaitTimeForAutounlocking;
            public GameObject LockGo;
        }

        
        //正在解锁
        bool unlocking;
        bool InCameraLockState;
        bool LockableAfterWaitingTime;
        
        int BorderCrossings;
        
        Coroutine timerCoroutine;
        ZoneCinemachine LockGoCinemachine;
        ZoneCinemachine CurrentZoneCinemachine;
        ZoneCinemachine LastZoneCinemachine;


        #region 初始化与获取

        public CinemachineVirtualCamera GetirtualCamera()
        {
            return CurrentZoneCinemachine.VirtualCamera;
        }
        public bool GetLock()
        {
            return InCameraLockState;
        }

        public void AddBorderCrossings()
        {
            BorderCrossings++;
        }
        public void SubBorderCrossings()
        {
            BorderCrossings--;
        }
        
        void InitLockCamera()
        {
            LockGoCinemachine = CamLock.LockGo.GetComponent<ZoneCinemachine>();
        }
        
        #endregion
        
        public void RegisterGo(GameObject ZonCam)
        {
            //121
            if(CurrentZoneCinemachine!=null  && ZonCam.name.Equals(CurrentZoneCinemachine.name) )
                return;
            
            LastZoneCinemachine = CurrentZoneCinemachine;
            CurrentZoneCinemachine = ZonCam.GetComponent<ZoneCinemachine>();
            // Debug.Log("ZonCam: " + ZonCam.name);
        }
        
        
        
        public void SwitchCameraLock()
        {
            if (BorderCrossings >= 3)
            {
                if (InCameraLockState)
                    return;
               
                CameraLockEnter();
                InCameraLockState = true;
            }
            // else
            // {
            //     if (!cameraLock && !CinemachineManager.Self.GetLock())
            //         return;
            //     
            //     Debug.Log("相机退出");
            //     CinemachineManager.Self.CameraLockExit();
            //     cameraLock = false;
            // }
        }
        

        private void CameraLockEnter()
        {
            //是否锁定相机
            // if(!LockableAfterWaitingTime)
            //     return;
            // LockableAfterWaitingTime = false;
            
            Debug.Log("相机锁定");
            LockEnter();
            
            //未解锁
            if (!unlocking)
            {
                if (timerCoroutine != null)
                    StopCoroutine(timerCoroutine);
                timerCoroutine = StartCoroutine(MathCameraAndPlayDistance());
            }
            
        }

        private void CameraLockExit()
        {
            if (CurrentZoneCinemachine.InTrigger)
            {
                SetVirtualCamera(CurrentZoneCinemachine.VirtualCamera);
                CurrentZoneCinemachine.ZoneCamEnter();
                
                
                // Debug.Log("解锁 " + CurrentZoneCinemachine.name);
            }
            else
            {
                SetVirtualCamera(LastZoneCinemachine.VirtualCamera);
                LastZoneCinemachine.ZoneCamEnter();
                
                
                // Debug.Log("解锁 " + LastZoneCinemachine.name);
            }
            
            //解锁完成
            LockExit();
            unlocking = false;
            InCameraLockState = false;
            //3S 后可以再次锁定
            StartCoroutine(WaitTimeForAgainCanLockCamera());
        }

        private void LockEnter()
        {
            LockGoCinemachine.transform.position = Self.transform.position;
            LockGoCinemachine.ZoneCamEnter();
        }

        private void LockExit()
        {
            LockGoCinemachine.ZoneCamExit();
        }


        IEnumerator MathCameraAndPlayDistance()
        {
            //正在解锁
            unlocking = true;
            
            float timeout = Time.time + CamLock.cameraWaitTimeForAutounlocking;
            while (Time.time < timeout)
            {
                // Debug.Log(GetCameraAndPlayDistance() > CamLock.SlowMitonThresholdDistance);
                // Debug.Log("距离：" +  GetCameraAndPlayDistance());
                if (GetCameraAndPlayDistance() > CamLock.SlowMitonThresholdDistance)
                {
                    Debug.Log("距离解锁");
                    CameraLockExit();
                    yield break;
                }

                yield return null;
            }

            Debug.Log(CamLock.cameraWaitTimeForAutounlocking + "秒已过，自动解锁");
            if (InCameraLockState)
                CameraLockExit();
        }

        IEnumerator WaitTimeForAgainCanLockCamera()
        {
            // 等待三秒
            yield return new WaitForSeconds(3.0f);
            LockableAfterWaitingTime = true;
        }
       
        
        private float GetCameraAndPlayDistance()
        {
            return (float)
            (
                math.abs(Math.Round(
                    Vector2.Distance(PlayerManager.Self.GetPlayer().transform.position, Self.transform.position)
                    , 2))
            );
        }

        #endregion


        //========================================================================


#if UNITY_EDITOR

        #region Editor

        private void SaveData()
        {
            CinemachineManagerData.m_CameraParameter = m_CameraParameter;
            CinemachineManagerData.m_Blend = m_Blend;
            CinemachineManagerData.CamLock = CamLock;
        }

        private void GetData()
        {
            m_CameraParameter = CinemachineManagerData.m_CameraParameter;
            m_Blend = CinemachineManagerData.m_Blend;
            CamLock = CinemachineManagerData.CamLock;
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

        protected virtual void OnDrawGizmos()
        {
            if(!IsDrew) return;
            // 查找场景中第一个带有特定脚本的物体
            PlayerManager foundObject = GameObject.FindObjectOfType<PlayerManager>();
            Vector3 Pos;
            // 检查是否找到物体
            if (foundObject != null)
            {
                 Pos = foundObject.GetPlayer().transform.position;
            }
            else
            {
                return;
            }
           
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Pos,CamLock.SlowMitonThresholdDistance);
        }
        
        
        #endregion

#endif
    }
}