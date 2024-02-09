using System;
using UnityEngine;

namespace CustomPlugins
{
    using ForegroundShelter;
    public class ShelterDelegate : MonoBehaviour
    {
        Shelte m_Shelte;
        public void SetKeyString(Shelte Target)
        {
            m_Shelte = Target;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            m_Shelte.CompareStrings(true);
        }
        

        private void OnTriggerExit(Collider other)
        {
            m_Shelte.CompareStrings(false);
        }
    }
}
