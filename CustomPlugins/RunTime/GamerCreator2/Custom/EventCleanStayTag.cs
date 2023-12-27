using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;
using System.Collections.Generic;

[Title("清除区域内tag对象并关闭自己")]
[Category("物理/清除区域内tag对象并关闭自己")]
//[Description("Executed while a game object stays inside the Trigger collider")]

[Image(typeof(IconTriggerStay), ColorTheme.Type.Blue)]
[Parameter("YZ", "YZ")]
[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Parameter("Set", "Where the value is set")]
[Parameter("From", "The value that is set")]

[Keywords("Change", "Boolean", "Variable")]

[Serializable]

public class EventCleanStayTag : TEventPhysics
{

   [Header("对象标签")]
   [SerializeField] private NeatoTag m_Tag;
  
   [Header("监控对象列表")]
   public List<GameObject> _gameObjects;
  

   [Header("监控清除完关闭自己对象")]
   public GameObject _self_gameObjects;

   // [Header("清除后关闭自己这个控件")]
   //  [SerializeField] private bool m_AfterCleanDisable = false;


   // [Header("监控开关")]
   //  [SerializeField] private bool m_OnOff = true;
   
  
   protected override void OnAwake(Trigger trigger)
   {
      base.OnAwake(trigger);
      trigger.RequireRigidbody();
      _gameObjects = new List<GameObject>();
   }
    
   protected override void OnTriggerStay3D(Trigger trigger, Collider collider)
   {

     // if(!this.m_OnOff) return;

      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      // if (_gameObjects.Count > 0)
      // {
      //    int nullcount = 0;
      //    foreach (var go in _gameObjects)
      //    {
      //       if (go == null)
      //          nullcount++;
      //       //return;
      //    }
      //    GameObject targetLast = SignalProxy.GetProxyTarget(collider.gameObject);
      //    if (nullcount >= _gameObjects.Count)
      //    {
      //    //   this.m_OnOff = false;
      //       _gameObjects.Clear();
      //       GetGameObjectLastTriggerEnter.Instance = targetLast;
      //       _ = this.m_Trigger.Execute(targetLast);
      //    }
      // }

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;
      
       GameObject.Destroy(target);
      //  if(!_gameObjects.Contains(target))
      //        _gameObjects.Add(target);
      //  foreach(var go in _gameObjects){
      //     if (go != null)
      //       return;
      //  }
      // if (_gameObject != null)
      //    return;
      CoreManager.Self.RegisterTimer(0.1f, () =>
      {
         if (_gameObjects.Count > 0)
         {
            int nullcount = 0;
            foreach (var go in _gameObjects)
            {
               if (go == null)
                  nullcount++;
               //return;
            }
           // GameObject targetLast = SignalProxy.GetProxyTarget(collider.gameObject);
            if (nullcount >= _gameObjects.Count)
            {
               //   this.m_OnOff = false;
               _gameObjects.Clear();

               _self_gameObjects.SetActive(false);
               // GetGameObjectLastTriggerEnter.Instance = targetLast;
               // _ = this.m_Trigger.Execute(targetLast);
            }
         }
      });


      // GetGameObjectLastTriggerEnter.Instance = target;
      // _ = this.m_Trigger.Execute(target);

   }

   protected override void OnUpdate(Trigger trigger)
   {
      if (_gameObjects.Count > 0)
      {
         int nullcount = 0;
         foreach (var go in _gameObjects)
         {
            if (go == null)
               nullcount++;
         }
         if (nullcount >= _gameObjects.Count)
         {
            _gameObjects.Clear();

            _self_gameObjects.SetActive(false);
         }
      }
   }

   
   protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
   {
     // if(!this.m_OnOff) return;

      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;

      
      if(!_gameObjects.Contains(target))
             _gameObjects.Add(target);
   }
   
   protected override void OnTriggerExit3D(Trigger trigger, Collider collider)
   {
     // if(!this.m_OnOff) return;

    //  if(!this.m_CheckExit) return;
      
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if(_gameObjects.Contains(target))
             _gameObjects.Remove(target);
      
      if( _gameObjects.Count <= 0 ){
      //   this.m_OnOff = false;
        GetGameObjectLastTriggerEnter.Instance = target;
        _ = this.m_Trigger.Execute(target);
      }

   }
}
