using System;
using UnityEngine;


namespace CustomPlugins.ForegroundShelter
{
    public class ShelterEventArgs : EventArgs
    {
        public bool enter;
        public String keyString;
    }

    public class SheltePublisher : MonoBehaviour
    {
      
        public delegate void ShelterEventHandler(object source, ShelterEventArgs args);
        public static event ShelterEventHandler ShelterPublic;
        public void ShelterEncode(ShelterEventArgs args)
        {
            ShelterPublic?.Invoke(this, args);
        }
    }
    
   
}