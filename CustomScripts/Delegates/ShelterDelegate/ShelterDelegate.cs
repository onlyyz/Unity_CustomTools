using System;
using UnityEngine;

namespace CustomPlugins
{
    using ForegroundShelter;
    public class ShelterDelegate : MonoBehaviour
    {
        //发布
        SheltePublisher sheltePublisher = new SheltePublisher();
        public ShelterEventArgs Args = new ShelterEventArgs();
    
        private void OnEnable()
        {
            Debug.Log("订阅");
            sheltePublisher.ShelterEncoded += OnTriggerEncoded;
        }

        private void OnDisable()
        {
            Debug.Log("退订");
            sheltePublisher.ShelterEncoded -= OnTriggerEncoded;
        }
        private void OnTriggerEncoded(object source, ShelterEventArgs args)
        {
            Debug.Log("MailService ：" + args.keyString);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Args.keyString = transform.name;
            Debug.Log("触发委托");
            sheltePublisher.ShelterEncode(Args,true);
        }

        private void OnTriggerExit(Collider other)
        {
            Args.keyString = transform.name;
            Debug.Log("触发委托");
            sheltePublisher.ShelterEncode(Args,false);
        }
    }
}
