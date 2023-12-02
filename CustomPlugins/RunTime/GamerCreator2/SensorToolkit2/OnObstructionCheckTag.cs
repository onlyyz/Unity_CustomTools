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
        [SerializeField] public GameObject selfOBJ;
        [SerializeField] public GameObject parentOBJ;
        [SerializeField] public GameObject _lineEffectOBj;
        
        private bool isInstance = false;
        
        private GlobalNameVariables nameVar;
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
                isInstance = true;
                _lineEffect = selfOBJ.GetComponent<RaySensor>().InstaceEffect(_lineEffectOBj);
                _lineEffect.GetComponent<LaserControl>().InstanceOBJ(selfOBJ.transform.position ,selfOBJ.GetComponent<RaySensor>().Length * selfOBJ.transform.forward);
                _lineEffect.transform.parent = selfOBJ.transform.parent;
            }
            
            _lineEffect.SetActive(
                (bool)parentOBJ.transform.parent.GetComponent<GetGlobalNameData>().NameVar.Get(parentOBJ.name));
            
            raySensor.OnClear.AddListener(OnClear);
            raySensor.OnObstruction.AddListener(OnObstruction);
        }

        void OnObstruction(IRayCastingSensor sensor)
        {
            if (parentOBJ == null || selfOBJ == null|| _lineEffect ==null)
            {
                Debug.Log(parentOBJ.name + " 挂点OBJ 未赋予 ");
                return;
            }
            
            RayHit hit = sensor.GetObstructionRayHit();
            nameVar = parentOBJ.transform.parent.GetComponent<GetGlobalNameData>().NameVar;

            var lineRnder = _lineEffect.GetComponent<LaserControl>();
                        
            if (nameVar == null)
            {
                Debug.Log(parentOBJ.name + " Global Variables 变量未赋值，请设置" );
                return ;
            }
            if (nameVar.Get(parentOBJ.name) == null)
            {
                Debug.Log(parentOBJ.name + " Global Variables 变量不存在，请设置" );
                return;
            }
            if (!(bool)nameVar.Get(parentOBJ.name))
            {
                _lineEffect.SetActive(false);
                return;
            }
            else
            {
                _lineEffect.SetActive(true);
            }
            lineRnder.SetPointPos(selfOBJ.transform.position,hit.Point);
            
           Debug.Log("回溯");
            if(hit.Collider.gameObject.GetComponent<SignalProxy>().ProxyTarget.HasTag(this.m_Tag))
            {
                PlayerManager.Self.PlayerReturnLastPoint(type);
                
                return;
            };
            
            _ = m_Trigger.Execute();
        }
        void OnClear(IRayCastingSensor sensor)
        {
            _lineEffect.GetComponent<LaserControl>().LoseTarget(selfOBJ.transform,
                selfOBJ.GetComponent<RaySensor>().Length * selfOBJ.transform.forward);
            
            _ = m_Trigger.Execute();
        }
    }

}