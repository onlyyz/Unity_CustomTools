using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;
using NeatoTags.Core;

[Title("进入与退出触发")]
[Category("触发/Trigger进入与退出")]
//[Description("Executed while a game object stays inside the Trigger collider")]

[Image(typeof(IconEye), ColorTheme.Type.Yellow)]

[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Serializable]
public class TriggerEnterAndExit :  TEventPhysics
{
    public GameObject Self;
    [SerializeField] private NeatoTag m_Tag;
   
    [SerializeField,Header("进入触发")] protected InstructionList m_EnterInstruction = new ();

    [SerializeField,Header("退出触发")] protected InstructionList m_ExitInstruction = new ();
    
    [NonSerialized] private Args m_Args;
  
  
    private void Awake()
    {
        this.m_Args = new Args(Self);
    }
    
    protected override void OnTriggerEnter3D(Trigger trigger, Collider collider)
    {
        base.OnTriggerStay3D(trigger, collider);
        if (!this.IsActive) return;
        if (!SignalProxy.GetProxyTarget(collider.gameObject).HasTag(this.m_Tag)) return;
        
      
        this.m_EnterInstruction?.Run(this.m_Args);
        
        
        GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);
        _ = this.m_Trigger.Execute(target);
    }
    

    protected override void OnTriggerExit3D(Trigger trigger, Collider collider)
    {
        base.OnTriggerExit3D(trigger, collider);
        if (!this.IsActive) return;
        if (!SignalProxy.GetProxyTarget(collider.gameObject).HasTag(this.m_Tag)) return;
        
        
        this.m_ExitInstruction?.Run(this.m_Args);
        
        
        GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);
        _ = this.m_Trigger.Execute(target);
    }
    

}
