using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NeatoTags.Core;
using UnityEngine;

namespace Micosmo.SensorToolkit.GameCreator {

    [Title("Obst打中与击空 特效")]

    [Image(typeof(IconEye), ColorTheme.Type.Teal, typeof(OverlayArrowRight))]

    [Category("SensorToolkit/On Obstruction 测试")]
    [Description("Executes when the RaySensor is obstructed and was previously clear")]

    [Serializable]
    public class OnObstructionCheckTag : TEventSensor
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField] private NeatoTag m_Tag;
        [SerializeField,GCLabel("Self")] public GameObject thisGameObject;
        [SerializeField,GCLabel("特效")] public GameObject _lineEffectOBj;
        
        private bool isInstance = false;
        
        private GameObject thisObj;
        private GameObject _lineEffect;
        
        protected override void WhenDisabled(Trigger trigger, Sensor sensor) {
            var raySensor = sensor as IRayCastingSensor;
            if (raySensor == null) return;
            
            raySensor.OnClear.RemoveListener(OnClear);
            raySensor.OnObstruction.RemoveListener(OnObstruction);
        }

        protected override void WhenEnabled(Trigger trigger, Sensor sensor) {
            var raySensor = sensor as IRayCastingSensor;
            if (raySensor == null) return;

            
            
            if (!isInstance)
            {
                var trans = thisGameObject.transform;
                isInstance = true;
                _lineEffect = thisGameObject.GetComponent<RaySensor>().InstaceEffect(_lineEffectOBj);
                _lineEffect.GetComponent<LaserControl>().InstanceOBJ(
                    trans.position ,thisGameObject.GetComponent<RaySensor>().Length * trans.forward);
                _lineEffect.transform.parent = trans.parent;
            }
            
            
            raySensor.OnClear.AddListener(OnClear);
            raySensor.OnObstruction.AddListener(OnObstruction);
        }

        void OnObstruction(IRayCastingSensor sensor)
        {
            
            RayHit hit = sensor.GetObstructionRayHit();
            var lineRender = _lineEffect.GetComponent<LaserControl>();
            lineRender.SetPointPos(thisObj.transform.position,hit.Point);
            
            // Debug.Log(hit.Collider.name+ "  回溯");
            if(hit.Collider.gameObject.GetComponent<SignalProxy>().ProxyTarget.HasTag(this.m_Tag))
            {
                PlayerManager.Self.PlayerReturnLastPoint();
                return;
            };
            
            _ = m_Trigger.Execute();
        }
        void OnClear(IRayCastingSensor sensor)
        {
            _lineEffect.GetComponent<LaserControl>().LoseTarget(thisObj.transform,
                thisObj.GetComponent<RaySensor>().Length * thisObj.transform.forward);
            
            _ = m_Trigger.Execute();
        }
    }

}