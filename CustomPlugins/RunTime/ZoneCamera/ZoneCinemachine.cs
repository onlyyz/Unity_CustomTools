using System;
using Cinemachine;
using UnityEngine;


namespace CustomPlugins.Zone.Cinemachine
{
    using Manager;

    [RequireComponent(typeof(Collider))]
    public class ZoneCinemachine : MonoBehaviour
    {
        public CinemachineVirtualCamera VirtualCamera;
        public bool DrawGizmos = true;
        public bool IsSceneSwitch;
        [HideInInspector] public bool InTrigger;

        static GameObject _target;
        BoxCollider _boxCollider;
        CinemachineConfiner _cinemachineConfiner;

        private string zoneName;

        private static string
            _playerLayer = "PlayerController",
            _m_tag = "Player",
            _mGoTag = "SystemTrigger",
            _mColorTag = "/ZoneCam/",
            _mVirtualName = "Virtual Camera",
            _mLast = "/Last",
            _mCurrent = "/Current",
            _mIsTrigger = "/IsTrigger";

        #region Initialization

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

        #region Area camera

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag)) return;

            InTrigger = true;
            RegisterGo(collider.gameObject);

            if (CinemachineManager.Self.inSlowMotion) return;

            SetEnterBlendTime();
            ZoneCamEnter();
        }

        public void ZoneCamEnter()
        {
            VirtualCamera.name = _mColorTag + _mVirtualName;
            gameObject.name = _mCurrent + zoneName;


            VirtualCamera.gameObject.SetActive(true);
            VirtualCamera.Follow = _target.transform;
        }

        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag)) return;
            InTrigger = false;

            ZoneCamExit();
            SlowMotionExit();
        }

        public void ZoneCamExit()
        {
            VirtualCamera.name = _mVirtualName;
            gameObject.name = zoneName;
            InspectorColor();

            VirtualCamera.gameObject.SetActive(false);
            VirtualCamera.Follow = null;
        }

        #endregion

        #region SoomthCamreBlendTime

        void SetEnterBlendTime()
        {
            CinemachineManager.Self.SetVirtualCamera(VirtualCamera);
        }

        #endregion

        #region Slow Motion for Camera

        private void RegisterGo(GameObject target)
        {
            _target = target;
            CinemachineManager.Self.RegisterGo(this.gameObject);
        }

        public void SlowMotionEnter()
        {
            if (!InTrigger) return;
            // InspectorColor();


            VirtualCamera.gameObject.SetActive(true);
            VirtualCamera.Follow = _target.transform;
        }

        //121
        private void SlowMotionExit()
        {
            CinemachineManager.Self.GoSwarp();
            // CinemachineManager.Self.SlowMotionEnd();
        }

        #endregion

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