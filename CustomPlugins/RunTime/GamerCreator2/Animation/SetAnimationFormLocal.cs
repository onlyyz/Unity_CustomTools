using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
      
    [Title("设置动画Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("动画/设置动画Bool")]
    [Parameter("YZ", "YZ")]
    [Image(typeof(IconAnimator), ColorTheme.Type.Green)]
    
    [Serializable]
    public class SetAnimationFormLocal : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("Animation Bool")]private string Animation;
        [SerializeField,GCLabel("Bool名")] private PropertyGetString m_Localstring;
        
        public override string Title => $"{this}设置动画Bool";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string LoboolString = this.m_Localstring.Get(args);
            
            gameObject.GetComponent<Animator>().SetBool(
                Animation,
                (bool)gameObject.GetComponent<LocalNameVariables>().
                    Get(LoboolString));
            
            return DefaultResult;
        }
    }
}