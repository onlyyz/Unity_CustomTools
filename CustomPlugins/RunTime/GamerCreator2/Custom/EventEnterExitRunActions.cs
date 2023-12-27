using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;
using System.Collections.Generic;

[Title("Tag对象进入和离开区域执行Actions")]
[Category("物理/Tag对象进入和离开区域执行Actions")]
//[Description("Executed while a game object stays inside the Trigger collider")]

[Image(typeof(IconTriggerStay), ColorTheme.Type.Blue)]
[Parameter("YZ", "YZ")]
[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Parameter("Set", "Where the value is set")]
[Parameter("From", "The value that is set")]

[Keywords("Change", "Boolean", "Variable")]

[Serializable]

public class EventEnterExitRunActions : TEventPhysics
{

   [Header("对象标签")]
   [SerializeField] private NeatoTag m_Tag;
  
   [Header("监控对象列表")]
   public List<GameObject> _gameObjects;
  

  [SerializeField] [Header("进入区域执行Action")]
   private Actions m_EnterActions = new Actions();


  [SerializeField] [Header("离开区域执行Action")]
   private Actions m_ExitsActions = new Actions();


   // [Header("是否离开区域剔除监控对象")]
   //  [SerializeField] private bool m_CheckExit = false;


   // [Header("监控开关")]
   //  [SerializeField] private bool m_OnOff = true;
   
  
   protected override void OnAwake(Trigger trigger)
   {
      base.OnAwake(trigger);
      trigger.RequireRigidbody();
      _gameObjects = new List<GameObject>();
   }
    
   
   protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
   {
    //  if(!this.m_OnOff) return;

      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;

     if(!_gameObjects.Contains(target)){
         _gameObjects.Add(target);
         if(m_EnterActions!=null)
             m_EnterActions.Invoke(target);
     }

      GetGameObjectLastTriggerEnter.Instance = target;
      _ = this.m_Trigger.Execute(target);          
   }


   
   protected override void OnTriggerExit3D(Trigger trigger, Collider collider)
   {
      
      base.OnTriggerExit3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if(_gameObjects.Contains(target))
             _gameObjects.Remove(target);
      
      if( _gameObjects.Count <= 0 ){
      //   this.m_OnOff = false;
         if(m_ExitsActions!=null)
            m_ExitsActions.Invoke(target);

        GetGameObjectLastTriggerEnter.Instance = target;
        _ = this.m_Trigger.Execute(target);
      }

   }
}
