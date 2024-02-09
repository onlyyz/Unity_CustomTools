    using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
    using Sirenix.OdinInspector;
    using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Common.Title("设置物体Active")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/设置物体Active")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
   
    
    public class InsGameObjectActive : TInstructionGameObject
    {
        [GCLabel("设置活动状态")]public bool setActive;
    
        public override string Title => $"设置物体 {this.m_GameObject} 为Activce {this.setActive}";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            gameObject.SetActive(setActive);
            return DefaultResult;
        }
    }
}