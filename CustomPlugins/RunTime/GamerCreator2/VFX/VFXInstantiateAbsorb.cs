using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPlugins.Manager;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEngine;
using UnityEngine.VFX;
using Attribute = GameCreator.Runtime.Stats.Attribute;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Instantiate")]
    [Description("Creates a new instance of a referenced game object")]

    [Category("渲染_VFX/VFX吸收")]
    
    [Parameter("Game Object", "Game Object reference that is instantiated")]
    [Parameter("Position", "The position where the new game object is instantiated")]
    [Parameter("Rotation", "The rotation that the new game object has")]
    [Parameter("Save", "Optional value where the newly instantiated game object is stored")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Keywords("Create", "New", "Game Object")]
    [Serializable]
    public class VFXInstantiateAbsorb : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        [GCLabel("ATT")] public Attribute m_Attribute;
        [SerializeField,GCLabel("来源")] public PropertyGetGameObject m_Souce;
        [SerializeField,GCLabel("目标")] public PropertyGetGameObject m_Target;
      
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{m_Souce} 的 {m_Attribute.ID} 赋予{m_Target} ";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            GameObject sourceData = this.m_Souce.Get(args);
            GameObject targetData = this.m_Target.Get(args);
            RuntimeAttributeData sourceAttribute = sourceData.GetComponent<Traits>().RuntimeAttributes.Get(m_Attribute.ID);
            RuntimeAttributeData targetAttribute = targetData.GetComponent<Traits>().RuntimeAttributes.Get(m_Attribute.ID);
            targetAttribute.Value += sourceAttribute.Value;
            
            return DefaultResult;
        }

        
       
    }
}