using System;
using System.Collections;
using System.Collections.Generic;
using EventandDelegates;
using UnityEngine;

namespace DS
{
    public class TriggerSource : MonoBehaviour
    {
        public bool IsDrew;
        public bool IsCube;
        public bool IsSphere;
[Range(1.0f,10.0f)]
        public float Distance;
        public Vector3 ve3Disatance;
        //发布
        DelegatesEncode delegatesManager = new DelegatesEncode();

        private  TriggerEventArgs trigger = new TriggerEventArgs()
        {
            Name = "trigger"
        };
        private void OnEnable()
        {
            Debug.Log("订阅");
            delegatesManager.TriggerEncoded += OnTriggerEncoded;
        }

        private void OnDisable()
        {
            Debug.Log("退订");   
            delegatesManager.TriggerEncoded -= OnTriggerEncoded;
        }

        private void OnTriggerEnter(Collider other)
        {
            trigger.Name = transform.name;
            Debug.Log("触发委托");
            delegatesManager.Encode(trigger);
        }

        private void OnTriggerEncoded(object source, TriggerEventArgs args)
        {
            Debug.Log("MailService ：" + args.Name);
        }
        
        protected virtual void OnDrawGizmos()
        {
            if(!IsDrew) return;
            GetComponent<BoxCollider>().size = ve3Disatance;
            Gizmos.color = Color.red;
            if (IsCube)
            {
                Gizmos.DrawWireCube(transform.position,ve3Disatance);
            }
            if(IsSphere)
            {
                Gizmos.DrawWireSphere(transform.position,Distance);     
            }
           
        }
    }
}
