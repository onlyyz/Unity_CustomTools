using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("检查本地变量是否销毁自己")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/检查本地变量是否销毁自己")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class CheckLoaclBoolDestory : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        private LocalNameVariables NameVar;
        public string KeyString;
        [GCLabel("销毁的物体")]
        public PropertyGetGameObject m_destory;
        public override string Title => $"本地变量 {this.KeyString} 为true 销毁{this.m_GameObject}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            GameObject destory = this.m_destory.Get(args);
            NameVar = gameObject.GetComponent<LocalNameVariables>();
          
            if (!(bool)NameVar.Get(KeyString))
                return DefaultResult;
            
            //Destroy
            UnityEngine.Object.Destroy(destory);
            return DefaultResult;
        }
    }
}