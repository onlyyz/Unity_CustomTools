using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    [Title("Signal 设置动画Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/Signal设置动画Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconAnimator), ColorTheme.Type.Green)]
    
    [Serializable]
    public class SetAnimationBoolFormSignal : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("动画 bool")]private string Animation;
        [GCLabel("Signal key")]public string KeyString;
        public override string Title => $"{KeyString} 设置动画Bool {Animation}";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            
            foreach(var nv in args.variables)
            {
                if(nv.Name == KeyString)
                {
                    gameObject.GetComponent<Animator>().SetBool(
                        Animation,(bool)nv.Value);
                }
            }
            return DefaultResult;
        }
    }
}