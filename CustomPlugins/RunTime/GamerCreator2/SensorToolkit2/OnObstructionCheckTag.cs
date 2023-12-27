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
        [SerializeField]
        public string type;
        [SerializeField] private NeatoTag m_Tag;
        [SerializeField] public GameObject parentOBJ;
        [SerializeField] public GameObject _lineEffectOBj;
        
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

            
            thisObj = parentOBJ.GetComponentInChildren<RaySensor>().gameObject;
            
            if (!isInstance)
            {
                isInstance = true;
                _lineEffect = thisObj.GetComponent<RaySensor>().InstaceEffect(_lineEffectOBj);
                _lineEffect.GetComponent<LaserControl>().InstanceOBJ(
                    thisObj.transform.position ,thisObj.GetComponent<RaySensor>().Length * thisObj.transform.forward);
                _lineEffect.transform.parent = thisObj.transform.parent;
            }
            
            
            raySensor.OnClear.AddListener(OnClear);
            raySensor.OnObstruction.AddListener(OnObstruction);
        }

        void OnObstruction(IRayCastingSensor sensor)
        {
            if (parentOBJ == null || thisObj == null|| _lineEffect ==null)
            {
                Debug.Log(parentOBJ.name + " 挂点OBJ 未赋予 ");
                return;
            }
            
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