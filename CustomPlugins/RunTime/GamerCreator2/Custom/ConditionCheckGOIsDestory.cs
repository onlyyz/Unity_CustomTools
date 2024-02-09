using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

[Title("Attribute Decrease LastTime")]
[Description("Returns true if the Attribute Change")]

[Category("逻辑/GameObject是否为空")]
[Parameter("YZ", "YZ")]
[Keywords("Health", "Mana", "Stamina", "Magic", "Life", "HP", "MP")]
[Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    
[Serializable]
public class ConditionCheckGOIsDestory : Condition
{
    // MEMBERS: -------------------------------------------------------------------------------
        
    [SerializeField] private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();
    
    protected override string Summary => $"判断{this.m_GameObject}是否为空";

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
