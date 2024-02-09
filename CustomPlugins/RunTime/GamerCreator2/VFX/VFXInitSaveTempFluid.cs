using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using Sirenix.OdinInspector;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Common.Title("Instantiate")]
    [Description("Creates a new instance of a referenced game object")]

    [Category("渲染_VFX/实例特效预制体，暂存Fluid")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class VFXInitSaveTempFluid : InstructionTraits
    {
        
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField,Header("计算公式")] 
        private Formula m_Formula;
       
        [SerializeField,Header("特效")] 
        public PropertyGetTraits m_vfx;
        
        [SerializeField,GCLabel("生成位置")] 
        private PropertyGetLocation m_Location;
        
        public override string Title => string.Format(
            "生成特效 {0} 暂存 {1} Fluid",
            this.m_vfx,
            this.m_Traits
        );
        
        // RUN METHOD: ----------------------------------------------------------------------------
        protected override  Task Run(Args args)
        {
            GameObject VFX = this.m_vfx.GetTraitsGameObject(args);
            Location location = this.m_Location.Get(args);
            Vector3 position = location.HasPosition
                ? location.GetPosition(args.Self)
                : args.Self != null ? args.Self.transform.position : Vector3.zero;
            
            Quaternion rotation = location.HasRotation
                ? location.GetRotation(args.Self)
                : args.Self != null ? args.Self.transform.rotation : Quaternion.identity;
            
            
            GameObject instance = UnityEngine.Object.Instantiate(VFX, position, rotation);
            InterObject inter = instance.GetComponent<InterObject>();
            
            //formula
            GameObject source = m_Traits.GetTraitsGameObject(args);
            GameObject target = inter.GetTraitsGameObject();
            
          
            //Get Fluid
            RuntimeAttributeData vfXAttribute = inter.GetRuntimeAttributeData(m_vfx.GetAttributeID());
            // this.m_Formula.Calculate(source, target);
            vfXAttribute.Value =  this.m_Formula.Calculate(source, target);
            return DefaultResult;
        }

      
    }
}