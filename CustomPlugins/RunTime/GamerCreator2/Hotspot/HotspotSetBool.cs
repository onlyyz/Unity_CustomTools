using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Hotspot Set local Bool")]
    [Category("赋值/Hotspot Set local Bool")]
    [Description("Executed when its associated Hotspot is activated")]

    [Image(typeof(IconHotspot), ColorTheme.Type.Green)]
    [Parameter("YZ", "YZ")]
    [Keywords("Spot")]

    [Serializable]
    public class HotspotSetBool : Event
    {
        [SerializeField] private PropertyGetGameObject m_Hotspot = GetGameObjectSelf.Create();
        [NonSerialized] private Hotspot m_Cache;

        private LocalNameVariables localName;
        [SerializeField,GCLabel("本地变量名")] public string m_string;
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            this.m_Cache = this.m_Hotspot.Get<Hotspot>(this.Self);
            if (this.m_Cache == null) return;
            
            localName = this.Self.GetComponent<LocalNameVariables>();
            
            this.m_Cache.EventOnActivate -= this.OnEnter;
            this.m_Cache.EventOnActivate += this.OnEnter;
            
            this.m_Cache.EventOnDeactivate -= this.OnExit;
            this.m_Cache.EventOnDeactivate += this.OnExit;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            if (this.m_Cache == null) return;
            this.m_Cache.EventOnActivate -= this.OnEnter;
            this.m_Cache.EventOnDeactivate -= this.OnExit;
        }

        private void OnEnter()
        {
            localName.Set(m_string,true);
            _ = this.m_Trigger.Execute(this.Self);
        }
        
        private void OnExit()
        {
            localName.Set(m_string,false);
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}