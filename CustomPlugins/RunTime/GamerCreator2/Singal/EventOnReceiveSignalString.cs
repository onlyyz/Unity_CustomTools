using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;
using GameCreator.Runtime.VisualScripting;
using Sirenix.OdinInspector;


[GameCreator.Runtime.Common.Title("接受Signal和比较String")]
    [Category("逻辑/接受Signal和比较String")]
    [Description("Executed when receiving a specific signal name from the dispatcher")]
    [Image(typeof(IconSignal), ColorTheme.Type.Red)]
    [Keywords("Event", "Command", "Fire", "Trigger", "Dispatch", "Execute")]
    [Parameter("YZ", "YZ")]
    
    
    [Serializable]
    public class EventOnReceiveSignalString : GameCreator.Runtime.VisualScripting.Event
    {
        LocalNameVariables NameVar;
        [SerializeField,LabelText("触发函数")] private Signal m_Signal;
        [SerializeField,GCLabel("Key 信息")] private string m_string;
        [SerializeField,GCLabel("本地 bool")] private string m_key;
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
            // NameVar = .GetComponent<LocalNameVariables>().
            
            var boolString = (string)NameVar.Get(m_string);
            
            
            
            if (!boolString.Equals(isString))
            {
                NameVar.Set((string)NameVar.Get(m_key),false);
                return;
            }
            
            NameVar.Set((string)NameVar.Get(m_key),true);
            
           
            _ = this.m_Trigger.Execute(args.invoker);
        }
    }
