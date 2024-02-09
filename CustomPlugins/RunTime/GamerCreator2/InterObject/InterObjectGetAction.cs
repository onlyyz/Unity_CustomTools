using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]

    [Title("Inter Object Run Actions")]
    [Description("Executes an Actions component object")]

    [Category("Inter Object/Run Actions")]

    [Image(typeof(IconInstructions), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InterObjectGetAction : InstructionEventOrAction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        [SerializeField,GCLabel("Action 类型")] 
        public ActionType actionType;
        [SerializeField,GCLabel("自定义Action")] private PropertyGetGameObject m_Actions = GetGameObjectActions.Create();
        [SerializeField,GCLabel("等待Action运行结束")] private bool m_WaitToFinish = true;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Run {0} {1}", 
            this.m_Actions,
            this.m_WaitToFinish ? "and wait" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            interObject = GetInterObject(args);
            actions = this.m_Actions.Get<Actions>(args);
       
            
            SwitchActionType(actionType, args);
            
            if (actions == null) return;
            if (this.m_WaitToFinish) await actions.Run(args);
            else _ = actions.Run(args);
        }

        
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