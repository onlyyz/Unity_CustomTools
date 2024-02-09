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

    [Category("InterObject/UITest")]

    [Image(typeof(IconInstructions), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class UITest : InstructionEventOrAction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        // 确保这些枚举和字段在Inspector中是可见的
        [SerializeField, GCLabel("目标数量")]
        public SignalType signalType;
    
        // Option A Parameters
        [SerializeField, GCLabel("Action 类型")] 
        public ActionType actionType;
        [SerializeField, GCLabel("自定义Action")] 
        public PropertyGetGameObject m_ActionGameObject = GetGameObjectActions.Create();
        [SerializeField, GCLabel("等待Action运行结束")] 
        public bool m_WaitToFinish = true;
    
        // Option B Parameters
        [SerializeField, GCLabel("事件名")] 
        public Signal m_Signal;
        [SerializeField, GCLabel("数据")]  
        public List<NameVariable> m_Data = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "传输信号{0}{1}", 
            this.signalType.ToString(),
            this.m_WaitToFinish ? "and wait" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            interObject = GetInterObject(args);
            actions = this.m_ActionGameObject.Get<Actions>(args);
            
            SwitchSignalType(signalType,actionType, args);
            
            if (actions == null) return;
            if (this.m_WaitToFinish) await actions.Run(args);
            else _ = actions.Run(args);
        }
        
        //====================================================
        public override void SwitchSignalMultiTarget(Args args)
        {
            
        }
        
        
        //====================================================
        public override void SwitchActionSeedSignal(Args args)
        {
            actions = interObject.GetActions(false);
        }
        
        public override void SwitchActionAcceptSignal(Args args)
        {
            actions = interObject.GetActions();
        }
        public override void SwitchActionCustomAction(Args args)
        {}
        
      
    }
}