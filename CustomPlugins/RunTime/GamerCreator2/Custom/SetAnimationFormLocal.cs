using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("设置 Local Animation Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/设置 Local Animation Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class SetAnimationFormLocal : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("Animation Bool")]private string Animation;
        [SerializeField,GCLabel("Bool名")] private PropertyGetString m_Localstring;
       
        LocalNameVariables LocalNameVar;
        public override string Title => $"设置Local Animation Bool";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string LoboolString = this.m_Localstring.Get(args);
            
            LocalNameVar = gameObject.GetComponent<LocalNameVariables>();
            gameObject.GetComponent<Animator>().SetBool(Animation,(bool)LocalNameVar.Get(LoboolString));
            return DefaultResult;
        }
    }
}