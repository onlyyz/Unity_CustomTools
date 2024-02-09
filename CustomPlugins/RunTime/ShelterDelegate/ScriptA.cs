using UnityEngine;

namespace CustomPlugins.ForegroundShelter
{
    public class ScriptA : MonoBehaviour
    {
        public string KeyString;

        private void OnEnable()
        {
            SheltePublisher.ShelterPublic += HandleMessage;
        }

        private void OnDisable()
        {
            SheltePublisher.ShelterPublic -= HandleMessage;
        }

        void HandleMessage(object source, ShelterEventArgs args)
        {
            Debug.Log(KeyString + ": " + args.keyString);
        }
    }
}