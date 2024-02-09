using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("Set Material Color")]
    [Description("Destroys a game object scene instance")]
    [Category("渲染_VFX/Set Material Color")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class insMaterialLocalNameBoolSetColor : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField,GCLabel("颜色")]public PropertyGetColor m_color;
        [SerializeField,GCLabel("变量名")]public PropertyGetString m_string;
        public override string Title => $"设置{this.m_GameObject} materials color为 {this.m_color}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            if (this.m_GameObject.Get(args) == null) return DefaultResult;
            this.m_GameObject.Get(args).GetComponent<Renderer>().material.SetColor
            (
                this.m_string.Get(args),this.m_color.Get(args)
            );
            
            return DefaultResult;
        }
    }
}