using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    [Title("String 设置动画Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("动画/String 设置动画Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconAnimator), ColorTheme.Type.Green)]
    
    [Serializable]
    public class SetAnimationFormLocalString : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("动画 bool")]private string Animation;
        [GCLabel("本地 key")]public string KeyString;
        
        public override string Title => $"{KeyString} 设置动画Bool";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            
            gameObject.GetComponent<Animator>().SetBool(
                Animation,
                (bool)gameObject.GetComponent<LocalNameVariables>().
                    Get(KeyString));
            
            return DefaultResult;
        }
    }
}