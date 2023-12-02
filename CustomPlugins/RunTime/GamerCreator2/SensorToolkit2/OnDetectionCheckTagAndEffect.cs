using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Lightbug.CharacterControllerPro.Core;
using NeatoTags.Core;
using UnityEngine;

namespace Micosmo.SensorToolkit.GameCreator {

    [Title("射线判断Tag并播放特效")]
    [Image(typeof(IconEye), ColorTheme.Type.Teal, typeof(OverlayPlus))]
    [Category("SensorToolkit/射线检测判断Tag并播放特效")]
    [Description("Executes when the Sensor detects a new game object")]

    [Serializable]
    public class OnDetectionCheckTagAndEffect : TEventSensor
    {
        [SerializeField] private NeatoTag m_Tag;
        [SerializeField]
        PropertySetGameObject m_StoreDetection = new PropertySetGameObject();

        protected override void WhenDisabled(Trigger trigger, Sensor sensor) {
            sensor.OnSignalAdded -= OnDetection;
        }
        protected override void WhenEnabled(Trigger trigger, Sensor sensor) {
            sensor.OnSignalAdded += OnDetection;
        }

        void OnDetection(Signal signal, Sensor sensor) {
            m_StoreDetection.Set(signal.Object, m_Trigger);
            _ = m_Trigger.Execute(signal.Object);


            // var collider = signal.Object.GetComponent<CharacterBody>().bodySize;
            
            
            
            Debug.Log(signal.Object.name);
            // GameObject _gameobj = new GameObject("Test");
            // _gameobj.transform.position = signal.Object.transform.position;
                
            
            if(!signal.Object.HasTag(this.m_Tag)) return;
           
          
           
        }
    }

}