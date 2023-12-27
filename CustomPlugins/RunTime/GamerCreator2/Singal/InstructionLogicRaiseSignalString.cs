using System;
using System.Threading.Tasks;
#if  UNITY_EDITOR
using GameCreator.Editor.Common;
#endif
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("发送信号和String")]
    [Description("Emits a specific signal, which is captured by other listeners")]

    [Category("逻辑/发送信号和String")]

    [Parameter("Signal", "The signal name emitted")]

    [Keywords("Event", "Raise", "Command", "Fire", "Trigger", "Dispatch", "Execute")]
    [Parameter("YZ", "YZ")]
    
    [Image(typeof(IconSignal), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionLogicRaiseSignalString : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField,GCLabel("函数名")] private Signal m_Signal;
        [SerializeField,GCLabel("Bool名")] private PropertyGetString m_string;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title
        {
            get
            {
                string signal = this.m_Signal.ToString();
                return string.IsNullOrEmpty(signal) ? "Signal (none)" : $"Signal '{signal}和' {m_string}";
            }
        }

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            string singalString = this.m_string.Get(args);
            SignalArgs data = new SignalArgs(this.m_Signal.Value, args.Self);
            Signals.Emit(data,singalString);
            return DefaultResult;
        }
    }
}