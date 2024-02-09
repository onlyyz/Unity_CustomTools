using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Game Object", "Target Event Or Action")]

    [Serializable]
    public abstract class InstructionEventOrAction : InstructionInterObject
    
    {
        public enum ActionType
        {
            发射信号,
            接受信号,
            自定义
        }
        
        public enum SignalType
        {
            单目标,
            多目标
        }

        public void SwitchSignalType(SignalType signalType,ActionType actionType,Args args)
        {
            switch (signalType)
            {
                case SignalType.单目标:
                    SwitchActionType(actionType, args);
                    break;
                case SignalType.多目标:
                    SwitchSignalMultiTarget(args);
                    break;
                default:
                    break;
            }
        }
    
        public virtual void SwitchSignalMultiTarget(Args args)
        {}
        
        
        public void SwitchActionType(ActionType actionType,Args args)
        {
            switch (actionType)
            {
                case ActionType.发射信号:
                    SwitchActionSeedSignal(args);
                    break;
                case ActionType.接受信号:
                    SwitchActionAcceptSignal(args);
                    break;
                case ActionType.自定义:
                    SwitchActionCustomAction(args);
                    break;
                default:
                    break;
            }
        }
        
        public virtual void SwitchActionSeedSignal(Args args)
        {}
        public virtual void SwitchActionAcceptSignal(Args args)
        {}
        public virtual void SwitchActionCustomAction(Args args)
        {}
        
        
    }
}