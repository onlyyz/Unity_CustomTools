using System;

using UnityEngine;
using UnityEngine.Events;

namespace DS
{
    public class TriggerEventArgs : EventArgs
    {
        public String Name;
    }
    public class DelegatesEncode : MonoBehaviour
    {
        //1 - 决定了订阅的形式
        public delegate void TriggerEncodeEventHandler(object source, TriggerEventArgs args);
        //2 - 创建事件
        public event TriggerEncodeEventHandler TriggerEncoded;

        public static event Action OnUnVariable;
        public static event Action<float> OnVariable;
        public UnityEvent OnUnityEvent;
        public void Encode(TriggerEventArgs args)
        {
            // OnVariable?.Invoke(float);
            OnUnityEvent.Invoke();
            OnTriggerEncode(args);
        }
        protected virtual void OnTriggerEncode(TriggerEventArgs args)
        {
            if (TriggerEncoded != null)
                TriggerEncoded(this, new TriggerEventArgs(){Name = args.Name});
        }
    }
}
