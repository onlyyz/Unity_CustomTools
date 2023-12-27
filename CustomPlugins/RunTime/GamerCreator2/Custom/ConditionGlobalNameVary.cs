using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

[Title("判断变量")]
[Description("Returns true if the Attribute Change")]

[Category("逻辑/GlobalName")]
[Parameter("YZ", "YZ")]
[Keywords("Health", "Mana", "Stamina", "Magic", "Life", "HP", "MP")]
[Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    
[Serializable]
public class ConditionGlobalNameVary : Condition
{
    // MEMBERS: -------------------------------------------------------------------------------
        
    public GlobalNameVariables NameVar;
    [SerializeField] public string type;
    protected override string Summary => $"判断变量{NameVar.name} {type}";
    // PROPERTIES: ----------------------------------------------------------------------------

    protected override bool Run(Args args)
    {
        if ((bool)NameVar.Get(type))
        {
            return true;
        }
        return false;
    }
}