using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;

[Title("Tag人物在区域击毁物体")]
[Category("物理/Tag人物在区域攻击销毁")]
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
   [SerializeField] private NeatoTag m_Tag;
   public GameObject _gameObject;
  
    [Header("Variables")]
   [SerializeField] 
   private PropertySetBool m_Set = SetBoolNone.Create;
        
   [SerializeField]
   private PropertyGetBool m_From = new PropertyGetBool();
   
   
  
   protected override void OnAwake(Trigger trigger)
   {
      base.OnAwake(trigger);
      trigger.RequireRigidbody();
   }
    
   protected override void OnTriggerStay3D(Trigger trigger, Collider collider)
   {
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;
       
      if (_gameObject != null)
         return;
       
      
      GetGameObjectLastTriggerEnter.Instance = target;
      _ = this.m_Trigger.Execute(target);

   }
   
   protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
   {
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;

      
      
      
      
      
      if (_gameObject != null)
         return;

      GetGameObjectLastTriggerEnter.Instance = target;
      _ = this.m_Trigger.Execute(target);
   }
   
   protected override void OnTriggerExit3D(Trigger trigger, Collider collider)
   {
      base.OnTriggerEnter3D(trigger, collider);

      if (!this.IsActive) return;

      GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

      if (!target.HasTag(this.m_Tag)) return;

      Debug.Log("Test Exit");
      
     
    
      if (_gameObject != null)
         return;
      
      GetGameObjectLastTriggerEnter.Instance = target;
      _ = this.m_Trigger.Execute(target);
   }
}
