using UnityEngine;
using System.Collections;
using Cinemachine;


namespace CustomPlugins.Zone.Cinemachine
{
    using Manager;

    [RequireComponent(typeof(Collider))]
    public class ZoneCinemachine : MonoBehaviour
    {
        #region Panel variable
        
        [HideInInspector] public bool InTrigger;
        [HideInInspector] public string zoneName;
        
        
        public CinemachineVirtualCamera VirtualCamera;
        public bool DrawGizmos = true;
        
        BoxCollider _boxCollider;
        CinemachineConfiner _cinemachineConfiner;
        
        static GameObject _target;
        static string
            _playerLayer = "PlayerController",
            _m_tag = "Player",
            _mGoTag = "SystemTrigger",
            _mVirtualName = "Virtual Camera",
          
            
            
            _mColorTag = "/ZoneCam/",
            _mLast = "/Last",
            _mCurrent = "/Current",
            _mIsTrigger = "/IsTrigger";
        
        #endregion
        
        //========================================================================
        
        
        #region 初始化

        private void Awake()
        {
            Initialization();
        }

        private void OnEnable()
        {
            Initialization();
        }

        private void OnDisable()
        {
            transform.name = zoneName;
        }

        protected virtual void Initialization()
        {
            if (VirtualCamera == null)
                VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(true);

            VirtualCamera.gameObject.GetComponent<CinemachineVirtualCamera>().enabled = true;
            VirtualCamera.transform.position = Vector3.zero;

            if (_cinemachineConfiner == null)
                _cinemachineConfiner = GetComponentInChildren<CinemachineConfiner>(true);

            var getBoxBool = transform.GetChild(0).GetComponent<BoxCollider>() != null;
            _boxCollider = (getBoxBool ? transform.GetChild(0) : transform.GetChild(1)).GetComponent<BoxCollider>();


            gameObject.layer = LayerMask.NameToLayer(_mGoTag);
            zoneName = transform.name;
        }

        #endregion
  
        
        //========================================================================

        
        #region Base Event Function

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag)) 
                return;
            InTrigger = true;
          
            CinemachineOnEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag))
                return;
            InTrigger = false;
            
            CinemachineOnExit();
        }
        
        public void ZoneCamEnter()
        {
            // Debug.Log("进入");
            //VirtualCamera.name = _mColorTag + _mVirtualName;
            //gameObject.name = _mCurrent + zoneName;
  
            VirtualCamera.Follow = _target.transform;
            VirtualCamera.gameObject.SetActive(true);
            
        }
        public void ZoneCamExit()
        {
            // Debug.Log("退出");
            //VirtualCamera.name = _mVirtualName;
            //gameObject.name = zoneName;
            //InspectorColor();
          
            VirtualCamera.gameObject.SetActive(false);
            VirtualCamera.Follow = null;
        }
        
        #endregion
        
        
        //========================================================================
        
        
        #region Cinemachine Camera
        
        private void SetEnterBlendTime()
        {
            CinemachineManager.Self.SetVirtualCamera(VirtualCamera);
        }
        
        private void CinemachineOnEnter(Collider collider)
        {
            _target = collider.gameObject;
            
            if (!CinemachineManager.Self.GetLock())
            {
                SetEnterBlendTime();
                ZoneCamEnter();
            }
            // else
            // {
            //     ZoneCamExit();
            // }
            
            //锁定相机
            CinemachineManager.Self.RegisterGo(gameObject);
            AddSubCameraNum(true);
        }

        //Get smooth Camera Blend Time
        private void CinemachineOnExit()
        {
            ZoneCamExit();
            
            //锁定相机
            StartCoroutine(DelaySubtractCameraNum());
        }
        
       
        IEnumerator DelaySubtractCameraNum()
        {
            yield return new WaitForSeconds(2.0f);
            AddSubCameraNum(false);
            // Debug.Log("后： " +  CameraNum);
        }
        #endregion
        

        //========================================================================
        
        
        #region 相机锁定

        private void AddSubCameraNum(bool addNum)
        {
            if(!CinemachineManager.Self.IsLockCamera)
                        return;
            if (addNum)
            {
                CinemachineManager.Self.AddBorderCrossings();
            }
            else
            {
                CinemachineManager.Self.SubBorderCrossings();
            }
            CinemachineManager.Self.SwitchCameraLock();
        }
        
        #endregion

        
        //========================================================================

        
#if UNITY_EDITOR
        private void InspectorColor()
        {
            if (InTrigger) gameObject.name = _mIsTrigger + zoneName;
            // Debug.Log(zoneName + ": " + InTrigger);
        }

        protected virtual void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            if (_boxCollider == null)
            {
                var getBoxBool = transform.GetChild(0).GetComponent<BoxCollider>() != null;
                _boxCollider = (getBoxBool ? transform.GetChild(0) : transform.GetChild(1)).GetComponent<BoxCollider>();
            }

            Gizmos.color = new Color(1f, 0.3f, 0.75f);
            Gizmos.DrawWireCube(_boxCollider.bounds.center, _boxCollider.bounds.size);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(gameObject.GetComponent<BoxCollider>().bounds.center,
                gameObject.GetComponent<BoxCollider>().bounds.size);
        }
#endif
    }
}