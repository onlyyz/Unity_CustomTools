using Cinemachine;
using UnityEngine;


namespace CustomPlugins.Zone.Cinemachine
{
    [RequireComponent(typeof(Collider))]
    public class ZoneCinemachine : MonoBehaviour
    {
        public LayerMask TriggerMask;
        public CinemachineVirtualCamera VirtualCamera;

        public bool DrawGizmos = true;

        //Zone Camera
        [HideInInspector] public string nameTag;
        [HideInInspector] public string GoName;

        BoxCollider _boxCollider;
        CinemachineConfiner _cinemachineConfiner;


        static string
            _playerLayer = "PlayerController",
            _m_tag = "Player";

        private void Awake()
        {
            Initialization();
        }

        private void OnEnable()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            if (VirtualCamera == null)
                VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(true);
            
            // Debug.Log(VirtualCamera);
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag)) return;
            ZoneCamEnter(collider);
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.tag.Equals(_m_tag)) return;
            ZoneCamExit(collider);
        }

        //TODO:tick the Camera
        public void ZoneCamEnter(Collider collider)
        {
            VirtualCamera.gameObject.SetActive(true);
            VirtualCamera.gameObject.GetComponent<CinemachineVirtualCamera>().enabled = true;
            VirtualCamera.Follow = collider.gameObject.transform;
            // transform.name =nameTag+ GoName;
        }

        public void ZoneCamExit(Collider collider)
        {
            VirtualCamera.gameObject.SetActive(false);
            VirtualCamera.Follow = null;
            // transform.name = GoName ;
        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            if (_boxCollider == null)
            {
               
                if (gameObject.transform.GetChild(1).GetComponent<BoxCollider>()!=null)
                {
                    _boxCollider = gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
                }
                else
                {
                    _boxCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
                }
               
            }

            Gizmos.color = new Color(1, 1, 0, 1);
            Gizmos.DrawWireCube(_boxCollider.bounds.center, _boxCollider.bounds.size);
            // Debug.Log(_boxCollider);

            Gizmos.color = new Color(0, 1, 1, 1);
            Gizmos.DrawWireCube(gameObject.GetComponent<BoxCollider>().bounds.center,
                gameObject.GetComponent<BoxCollider>().bounds.size);
        }

#endif
    }
}