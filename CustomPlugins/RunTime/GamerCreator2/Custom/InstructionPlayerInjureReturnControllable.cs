using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("玩家受击回到上一個回溯點")]
    [Description("回溯點")]

    [Category("回溯/角色受击回到回溯點")]
    
    [Parameter("Is FreezeCharacterPro", "是否锁定角色控制")]
    [Parameter("Is Remove Trigger", "是否删除触发器")]

    
    [Image(typeof(IconPlayer), ColorTheme.Type.Blue)]

    [Serializable]
public class InstructionPlayerInjureReturnControllable : Instruction
{
    // MEMBERS: -------------------------------------------------------------------------------

    // [SerializeField]
    // private PropertyGetGameObject m_Character = new PropertyGetGameObject();

    [SerializeField]
    public string type;
    
    // PROPERTIES: ----------------------------------------------------------------------------
        
    public override string Title => $"角色受击回到回溯點";

    // RUN METHOD: ----------------------------------------------------------------------------
        
    protected override Task Run(Args args)
    {
        PlayerManager.Self.PlayerReturnLastPoint(type);
        return DefaultResult;
    }
}
}