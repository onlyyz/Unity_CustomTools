using System;
using GameCreator.Runtime.Common;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;

[Title("碰撞器触发检查变量")]
[Category("物理/碰撞器检查变量")]
//[Description("Executed while a game object stays inside the Trigger collider")]
 
[Image(typeof(IconTriggerStay), ColorTheme.Type.Blue)]
[Parameter("YZ", "YZ")]
[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Serializable]

public class EventTriggerEnterCheckBool : TEventPhysics
{
   public GameObject _gameObject;
   protected override void OnAwake(Trigger trigger)
   {
      base.OnAwake(trigger);
      trigger.RequireRigidbody();
   }
    
   protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
   {
     
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;
      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);
      
       
      if(!collider.name.Equals("HitBox"))
         return;
     
      GetGameObjectLastTriggerEnter.Instance = target;
      _ = this.m_Trigger.Execute(target);
   }

}
