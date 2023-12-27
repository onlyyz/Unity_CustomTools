using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("bool 选择 材质颜色")]
    [Description("Destroys a game object scene instance")]
    [Category("渲染/bool 选择 材质颜色")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class insMaterialUseBoolSetColor : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField,GCLabel("True 颜色")]public PropertyGetColor m_color1;
        [SerializeField,GCLabel("False 颜色")]public PropertyGetColor m_color2;
        [SerializeField,GCLabel("变量名")]public PropertyGetString m_string;
        [SerializeField,GCLabel("Bool控制")]public PropertyGetBool m_bool;
        public override string Title => $"设置{this.m_GameObject} materials color";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            if (this.m_GameObject.Get(args) == null) return DefaultResult;
            var SelectColor = this.m_bool.Get(args);
            
            
            this.m_GameObject.Get(args).GetComponent<Renderer>().material.SetColor
            (
                this.m_string.Get(args),
                SelectColor?
                this.m_color1.Get(args):
                this.m_color2.Get(args)
            );
            
            return DefaultResult;
        }
    }
}