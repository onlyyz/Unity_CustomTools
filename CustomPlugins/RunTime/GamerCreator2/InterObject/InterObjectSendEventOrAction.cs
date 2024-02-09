using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]

    [Title("Run Actions")]
    [Description("Executes an Actions component object")]
    [Category("Inter Object/Actions 和 Signal")]

    [Image(typeof(IconMagic), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InterObjectSendEventOrAction : InstructionEventOrAction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        // 确保这些枚举和字段在Inspector中是可见的
        [SerializeField, GCLabel("信号接受类型")]
        public SignalType signalType;
    
        // Option A Parameters
        [SerializeField, GCLabel("Action 类型")] 
        public ActionType actionType;
        [SerializeField, GCLabel("自定义Action")] 
        public PropertyGetGameObject signalActions = GetGameObjectActions.Create();
        [SerializeField, GCLabel("等待Action运行结束")] 
        public bool m_WaitToFinish = true;
    
        // Option B Parameters
        [SerializeField, GCLabel("广播事件名")] 
        public Signal m_Signal;
        [SerializeField, GCLabel("数据")]  
        public List<NameVariable> m_Data = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title
        {
            get
            {
                string title = "信号: ";
                string single = actionType.ToString()+ " " + signalActions.ToString();
                
                string signal = this.m_Signal.ToString();
                string mulAction = string.IsNullOrEmpty(signal) ? "发送 (none)" :
                    $"发送 {signal} 事件，传输{m_Data.Count}个数据";
                return title + signalType.ToString() + (signalType.ToString().Equals("单目标") ? single: mulAction);
            }
        }

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            interObject = GetInterObject(args);
            actions = this.signalActions.Get<Actions>(args);
          
            SwitchSignalType(signalType,actionType, args);
            
            if (actions == null) return;
            if (this.m_WaitToFinish) await actions.Run(args);
            else _ = actions.Run(args);
        }
        
        //====================================================
        public override void SwitchSignalMultiTarget(Args args)
        {
            SignalArgs data = new SignalArgs(this.m_Signal.Value, args.Self,null,m_Data);
            Signals.Emit(data);
        }
        
        
        //====================================================
        public override void SwitchActionSeedSignal(Args args)
        {
            actions = interObject.GetActions();
        }
        
        public override void SwitchActionAcceptSignal(Args args)
        {
            actions = interObject.GetActions(false);
        }

        public override void SwitchActionCustomAction(Args args)
        {}
        
      
    }
}