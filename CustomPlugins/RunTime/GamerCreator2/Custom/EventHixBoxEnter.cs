using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using NeatoTags.Core;
using GameCreator.Runtime.VisualScripting;
using Micosmo.SensorToolkit;


[Title("HitBox物体进入触发")]
[Category("触发/HitBox物体进入触发")]
[Description("Executed when a game object with a Tag enters the Trigger collider")]

[Parameter("Tag", "A string that represents a group of game objects")]
[Image(typeof(IconTag), ColorTheme.Type.Green)]

[Keywords("Pass", "Through", "Touch", "Collision", "Collide")]

[Serializable]
public class EventHixBoxEnter : TEventPhysics
{
    [SerializeField] private NeatoTag m_Tag;

    private const string HitBox = "HitBox";
    // METHODS: -------------------------------------------------------------------------------

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
       
        if(! target.name.Equals(HitBox))
           return;
        
        GetGameObjectLastTriggerEnter.Instance = target;
        _ = this.m_Trigger.Execute(target);
    }

    protected override void OnTriggerEnter2D(Trigger trigger, Collider2D collider)
    {
        base.OnTriggerEnter2D(trigger, collider);

        if (!this.IsActive) return;

        GameObject target = SignalProxy.GetProxyTarget(collider.gameObject);

        if (!target.HasTag(this.m_Tag)) return;

        GetGameObjectLastTriggerEnter.Instance = target;
        _ = this.m_Trigger.Execute(target);
    }
}