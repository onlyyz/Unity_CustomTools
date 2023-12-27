using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;
using GameCreator.Runtime.VisualScripting;

    
    [Title("接受Signal和比较String")]
    [Category("逻辑/接受Signal和比较String")]
    [Description("Executed when receiving a specific signal name from the dispatcher")]
    [Image(typeof(IconSignal), ColorTheme.Type.Red)]
    [Keywords("Event", "Command", "Fire", "Trigger", "Dispatch", "Execute")]
    [Parameter("YZ", "YZ")]
    
    
    [Serializable]
    public class EventOnReceiveSignalString : GameCreator.Runtime.VisualScripting.Event
    {
        [SerializeField] private Signal m_Signal;
        public LocalNameVariables NameVar;
        [SerializeField,GCLabel("Loacl String名")] private string m_string;
        [SerializeField,GCLabel("Loacl Bool名")] private string m_key;
        protected  override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            Signals.Subscribe(trigger, m_Signal.Value);
        }

        protected  override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Signals.Unsubscribe(trigger, m_Signal.Value);
        }

        protected  override void OnReceiveSignalString(Trigger trigger, SignalArgs args ,string isString)
        {
            base.OnReceiveSignalString(trigger, args,isString);
            if (this.m_Signal.Value != args.signal) return;
            var boolString = (string)NameVar.Get(m_string);
            
            // Debug.Log("本地： " + boolString + "  传入： "  + isString);
            
            
            if (!boolString.Equals(isString))
            {
                NameVar.Set(m_key,false);
                return;
            }
            
            NameVar.Set(m_key,true);
            
           
            _ = this.m_Trigger.Execute(args.invoker);
        }
    }
