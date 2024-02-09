using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPlugins.Manager;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;
using UnityEngine.VFX;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Instantiate")]
    [Description("Creates a new instance of a referenced game object")]

    [Category("渲染_VFX/VFX吸收，设置Bool")]
    
    [Parameter("Game Object", "Game Object reference that is instantiated")]
    [Parameter("Position", "The position where the new game object is instantiated")]
    [Parameter("Rotation", "The rotation that the new game object has")]
    [Parameter("Save", "Optional value where the newly instantiated game object is stored")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Keywords("Create", "New", "Game Object")]
    [Serializable]
    public class VFXInstantiateAbsorbSetBool : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private VisualEffect m_vfx;
        [SerializeField] 
        private PropertyGetLocation m_Location = GetLocationCharacter.Create;
        [SerializeField,GCLabel("吸收消失时间")] 
        private float absorbTime;
        [SerializeField]
        protected FieldSetLocalName Variable = new FieldSetLocalName(ValueBool.TYPE_ID);
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"实例特效 {this.m_vfx} Set {this.Variable}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Location location = this.m_Location.Get(args);

            Vector3 position = location.HasPosition
                ? location.GetPosition(args.Self)
                : args.Self != null ? args.Self.transform.position : Vector3.zero;
            
            Quaternion rotation = location.HasRotation
                ? location.GetRotation(args.Self)
                : args.Self != null ? args.Self.transform.rotation : Quaternion.identity;
            
            VisualEffect instance = UnityEngine.Object.Instantiate(m_vfx, position, rotation);
            instance.gameObject.SetActive(true);
            // IEnumeratorManager.Self.EnemyDiedPlayerAbsorb(args,Variable,instance, absorbTime);
            
            return DefaultResult;
        }

        
       
    }
}