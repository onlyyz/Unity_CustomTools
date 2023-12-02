using NeatoTags.Core;
using UnityEngine;

namespace CustomPlugins.OneWayDoor
{
    public class SingleDoor : MonoBehaviour
    {
        [SerializeField] private NeatoTag m_Tag;
        [HideInInspector] public bool _isTrigger;
        public string _triggerMask = "PlayerController";
        private string TagObj = "2.CharactorData";
        public GameObject _bool;

        private void OnTriggerEnter(Collider other)
        {
            if (!LayerMask.LayerToName(other.gameObject.layer).Equals(_triggerMask)) return;
            if (!other.gameObject.transform.Find(TagObj).gameObject.HasTag(this.m_Tag)) return;
            _isTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!LayerMask.LayerToName(other.gameObject.layer).Equals(_triggerMask)) return;
            if (!other.gameObject.transform.Find(TagObj).gameObject.HasTag(this.m_Tag)) return;
            _isTrigger = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!LayerMask.LayerToName(other.gameObject.layer).Equals(_triggerMask)) return;
            if (!other.gameObject.transform.Find("2.CharactorData").gameObject.HasTag(this.m_Tag)) return;

            if (_bool) return;
            gameObject.GetComponent<Animator>().SetBool("Attack", true);

            Invoke("DestorySelf", 1.0f);
        }

        void DestorySelf()
        {
            Destroy(gameObject);
        }
    }
}