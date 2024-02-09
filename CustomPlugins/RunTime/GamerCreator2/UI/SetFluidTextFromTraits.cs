using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using TMPro;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Common.Title("设置TextMeshPro int")]
    [Description("Add two values together")]
    [Category("UI/设置TextMeshPro int")]
    
    [Keywords("Sum", "Plus", "Float", "Integer", "Variable")]
    [Image(typeof(IconPlusCircle), ColorTheme.Type.Blue)]
    
    [Serializable]

public class SetFluidTextFromTraits : InstructionTraits
{
    [SerializeField,GCLabel("字体物体")] 
    protected PropertyGetGameObject m_TextGameObject;
  
    public override string Title  => string.Format(
        ""
    );
        
    protected override Task Run(Args args)
    {
        GameObject m_TextGameObject = this.m_TextGameObject.Get(args);
        TextMeshPro textPro = m_TextGameObject.GetComponent<TextMeshPro>();
        
        int fluid = this.m_Traits.GetFluid(args);
        if(fluid == 0){
            textPro.text = "";
            return DefaultResult;
        }
        
        textPro.text = fluid.ToString(); 
        
        return DefaultResult;
    }
}
}