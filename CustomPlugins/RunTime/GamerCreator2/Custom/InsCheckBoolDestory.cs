using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("bool true 销毁自己")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/bool true 销毁自己")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class InsCheckBoolDestory : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        public PropertyGetBool m_bool;
        public override string Title => $"{this.m_bool} 为true 销毁 {this.m_GameObject}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            bool Switch = this.m_bool.Get(args);
            
            if (gameObject == null) return DefaultResult;
            
            if (!Switch)
                return DefaultResult;
            
            UnityEngine.Object.Destroy(gameObject);
            return DefaultResult;
        }
    }
}