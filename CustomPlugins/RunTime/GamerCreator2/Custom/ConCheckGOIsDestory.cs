using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

[Title("GO是否存在")]
[Description("Returns true if the Attribute Change")]

[Category("逻辑/GO是否存在")]
[Parameter("YZ", "YZ")]
[Keywords("Health", "Mana", "Stamina", "Magic", "Life", "HP", "MP")]
[Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    
[Serializable]
public class ConCheckGOIsDestory : Condition
{
    // MEMBERS: -------------------------------------------------------------------------------
        
    [SerializeField] private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

    // PROPERTIES: ----------------------------------------------------------------------------

    protected override bool Run(Args args)
    {
        GameObject gameObject = this.m_GameObject.Get(args);
        if (gameObject != null)
        {
            return true;
        }
        return false;
    }
}
