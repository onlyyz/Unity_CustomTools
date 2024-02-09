using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Game Object", "Target Traits")]

    [Serializable]
    public abstract class InstructionTraits : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        public PropertyGetTraits m_Traits;
        
        public void DebugLog(string str)
        {
            Debug.Log(str);
        }
    }
}