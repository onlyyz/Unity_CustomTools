using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Common.Title("抓取Inter Object NV")]
    [Description("Destroys a game object scene instance")]
    [Category("Inter Object/抓取Inter Object NV")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class InterObjectGetLocalNV : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [LabelText("Key")]
        private string KeyString;
     
       
        
        
        [SerializeField,Header("Inter Object")]
        protected FieldInterObjectSetLocalName variable = new FieldInterObjectSetLocalName(ValueBool.TYPE_ID);
        [SerializeField,Header("原生")]
        protected FieldSetLocalName variable1 = new FieldSetLocalName(ValueBool.TYPE_ID);

        //Boolean
       
        
        public override string Title => $"设置 本地变量{this.KeyString}";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            // GameObject gameObject = this.m_GameObject.Get(args);
            // gameObject.GetComponent<LocalNameVariables>().
            //     Set(
            //         KeyString,
            //         true
            //     );
            return DefaultResult;
        }
    }
}