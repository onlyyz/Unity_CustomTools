using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPlugins.Manager;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.VFX;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Instantiate")]
    [Description("Creates a new instance of a referenced game object")]

    [Category("渲染_VFX/设置特效值")]

    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Keywords("Create", "New", "Game Object")]
    [Serializable]
    public class VFXSetFloat : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField,GCLabel("特效")] 
        private VisualEffect m_vfx;
        [SerializeField,GCLabel("关键字")] 
        private PropertyGetString valueString;
        [SerializeField,GCLabel("值")] 
        private PropertyGetDecimal value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"设置特效的 {this.valueString} = {this.value} ";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            string str = this.valueString.Get(args);
            var vau =(float) this.value.Get(args);
            m_vfx.SetFloat(str,vau);
            //Debug.Log("str " + str + " = " + vau);
            
            
            
            return DefaultResult;
        }

        
       
    }
}