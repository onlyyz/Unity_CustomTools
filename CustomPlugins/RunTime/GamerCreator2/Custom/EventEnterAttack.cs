using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;
using System.Collections.Generic;

[Title("Tag对象组在区域内采集后全部销毁或离开区域")]
[Category("物理/Tag对象组在区域内采集后全部销毁或离开区域")]
//[Description("Executed while a game object stays inside the Trigger collider")]

[Image(typeof(IconTriggerStay), ColorTheme.Type.Blue)]
[Parameter("YZ", "YZ")]
[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Parameter("Set", "Where the value is set")]
[Parameter("From", "The value that is set")]

[Keywords("Change", "Boolean", "Variable")]

[Serializable]

public class EventEnterAttack : TEventPhysics
{

   [Header("对象标签")]
   [SerializeField] private NeatoTag m_Tag;
  
   [Header("监控对象列表")]
   public List<GameObject> _gameObjects;
  

   [Header("是否离开区域剔除监控对象")]
    [SerializeField] private bool m_CheckExit = false;


   [Header("监控开关")]
    [SerializeField] private bool m_OnOff = true;
   
  
   protected override void OnAwake(Trigger trigger)
   {
      base.OnAwake(trigger);
      trigger.RequireRigidbody();
      _gameObjects = new List<GameObject>();
   }
    
   protected override void OnTriggerStay3D(Trigger trigger, Collider collider)
   {

      if(!this.m_OnOff) return;

      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      if (_gameObjects.Count > 0)
      {
         int nullcount = 0;
         foreach (var go in _gameObjects)
         {
            if (go == null)
               nullcount++;
            //return;
         }
         GameObject targetLast = SignalProxy.GetProxyTarget(collider.gameObject);
         if (nullcount >= _gameObjects.Count)
         {
            //this.m_OnOff = false;
            _gameObjects.Clear();
            GetGameObjectLastTriggerEnter.Instance = targetLast;
            _ = this.m_Trigger.Execute(targetLast);
         }
      }

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;
       
       if(!_gameObjects.Contains(target))
             _gameObjects.Add(target);
      //  foreach(var go in _gameObjects){
      //     if (go != null)
      //       return;
      //  }
      // if (_gameObject != null)
      //    return;
       
      
      // GetGameObjectLastTriggerEnter.Instance = target;
      // _ = this.m_Trigger.Execute(target);

   }
   
   protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
   {
      if(!this.m_OnOff) return;

      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;

      
     if(!_gameObjects.Contains(target))
             _gameObjects.Add(target);
   }
   
   protected override void OnTriggerExit3D(Trigger trigger, Collider collider)
   {
      if(!this.m_OnOff) return;

      if(!this.m_CheckExit) return;
      
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if(_gameObjects.Contains(target))
             _gameObjects.Remove(target);
      
      if( _gameObjects.Count <= 0 ){
         this.m_OnOff = false;
        GetGameObjectLastTriggerEnter.Instance = target;
        _ = this.m_Trigger.Execute(target);
      }

   }
}
