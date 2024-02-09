using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using TMPro;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Common.Title("设置TextMeshPro Local NV")]
    [Description("Add two values together")]
    [Category("UI/设置TextMeshPro Local NV")]
    
    [Keywords("Sum", "Plus", "Float", "Integer", "Variable")]
    [Image(typeof(IconPlusCircle), ColorTheme.Type.Blue)]
    
    [Serializable]

    public class SetFluidTextFromLocalNV : Instruction
    {
       
        [SerializeField]
        protected FieldGetLocalName Variable = new FieldGetLocalName(ValueNumber.TYPE_ID);
     
        [SerializeField,GCLabel("字体物体")] 
        protected PropertyGetGameObject m_TextGameObject;
        public override string Title  => string.Format(
            ""
           
        );
        
        protected override Task Run(Args args)
        {
            GameObject m_TextGameObject = this.m_TextGameObject.Get(args);
            TextMeshPro textPro = m_TextGameObject.GetComponent<TextMeshPro>();
            double Value =(double)Variable.Get(args);
          
            if(Value == 0){
                textPro.text = "";
                return DefaultResult;
            }
            textPro.text =Value.ToString(); 
        
            return DefaultResult;
        }
    }
}