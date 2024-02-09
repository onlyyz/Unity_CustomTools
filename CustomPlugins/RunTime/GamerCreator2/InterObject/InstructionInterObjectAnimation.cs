using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Parameter("Game Object", "Target Animation")]
    [Serializable]
    public abstract class InstructionInterObjectAnimation : InstructionInterObject
    {
        [SerializeField,GCLabel("动画 物体")] 
        protected PropertyGetGameObject m_GameObject = new PropertyGetGameObject();
        protected Animator animaGameObject;
    }
}