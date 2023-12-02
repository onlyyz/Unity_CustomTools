using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NeatoTags.Core;
using UnityEngine;

namespace Micosmo.SensorToolkit.GameCreator {

    [Title("射线判断Tag")]

    [Image(typeof(IconEye), ColorTheme.Type.Teal, typeof(OverlayPlus))]

    [Category("SensorToolkit/射线检测同时判断Tag")]
    [Description("Executes when the Sensor detects a new game object")]

    [Serializable]
    public class SensorCheckTag : TEventSensor
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
            
            
            Debug.Log(signal.Object.name);
           if(!signal.Object.HasTag(this.m_Tag)) return;
           
        }
    }

}