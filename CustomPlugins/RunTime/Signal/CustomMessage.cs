using System;
using UnityEngine;

namespace CustomPlugins.Signal
{
    public class CustomMessage : CustomSignal
    {
        private static string _trigger = "SystemTrigger";

        private void Awake()
        {
            //TODO: remove this code , when editor can set the LayerMask
            if (LayerMask.LayerToName(gameObject.layer).Equals(_trigger))
                return;
            gameObject.layer = LayerMask.NameToLayer(_trigger);
        }

        void OnTriggerEnter(Collider collider)
        {
            SendMessageUpwards("TriggerEnterChild", collider);
        }

        void OnTriggerExit(Collider collider)
        {
            SendMessageUpwards("TriggerExitChild", collider);
        }
        
    }
}
