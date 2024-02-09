using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    [Title("InterObject 设置动画Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("Inter Object/设置动画Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconAnimator), ColorTheme.Type.Green)]
    [Serializable]
    public class InterObjectAnimSetBool : InstructionInterObjectAnimation
    {
        [SerializeField,GCLabel("Anim Var")] public string Animation;
        [SerializeField,GCLabel("变量值")] private PropertyGetBool Variable;
       
        
        // protected FieldSetLocalName Variable = new FieldSetLocalName(ValueBool.TYPE_ID);
        public override string Title => string.Format(
            "设置{0} 动画 {1} 状态为 {2} ",
            this.interObject,
            this.Animation,
            this.Variable
        );
       
        protected override Task Run(Args args)
        {
            interObject = GetInterObject(args);
            animaGameObject = interObject.GetAnimator();
            bool state = Variable.Get(args);
            
            if (animaGameObject == null)
            {
                animaGameObject = this.m_GameObject.Get(args).GetComponent<Animator>();
            }
            
            animaGameObject.SetBool(Animation, state);

            return DefaultResult;
        }
    }
}