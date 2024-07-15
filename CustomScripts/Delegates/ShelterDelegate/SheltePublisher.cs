using System;
using UnityEngine;
using System.Collections;

using Sirenix.OdinInspector;

namespace CustomPlugins.ForegroundShelter
{
    
    public class ShelterEventArgs : EventArgs
    {
        public String keyString;
    }
    
    
    public class SheltePublisher : MonoBehaviour
    {
        [LabelText("场景交互全局变量")]
        public String boolString;
        
        //1 - 决定了订阅的形式
        public delegate void ShelterEventHandler(object source, ShelterEventArgs args);
        //2 - 创建事件
        public event ShelterEventHandler ShelterEncoded;
        
        public void ShelterEncode(ShelterEventArgs args,bool Enter)
        {
            if (ShelterEncoded != null)
                ShelterEncoded(this, new ShelterEventArgs(){keyString = args.keyString});
            
            if (Enter && boolString.Equals(args.keyString))
            {
                Debug.Log("进入逻辑");
                // TriggerEnterChild();
            }
            else
            {
                Debug.Log("退出逻辑");
                // TriggerExitChild();
            }
        }
        
    }
}