using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Game Object", "Target InterObject")]

    [Serializable]
    public abstract class InstructionInterObject : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        [SerializeField,GCLabel("Inter 物体")] 
        protected PropertyGetGameObject m_interObject = new PropertyGetGameObject();
        [HideInInspector]public InterObject interObject;
        [HideInInspector]public Actions actions;
        public InterObject GetInterObject(Args args) => 
            this.m_interObject.Get(args).GetComponent<InterObject>();
        
    }
}