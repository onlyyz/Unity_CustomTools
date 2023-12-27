using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEngine;


namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("角色受击回到回溯點")]
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
    
    [SerializeField,GCLabel("伤害来源")]
    public BaseTypeandMult.AttackEnum attackOri;
    [SerializeField,GCLabel("伤害倍率")]
    public BaseTypeandMult.ManagerLevel ManagerLev;
    
    
    // PROPERTIES: ----------------------------------------------------------------------------
        
    public override string Title => $"角色受击回到回溯點";

    // RUN METHOD: ----------------------------------------------------------------------------
        
    protected override Task Run(Args args)
    {
        // Debug.Log(attackOri + "  " + ((int)ManagerLev+1));
        PlayerManager.Self.playerDamager(attackOri.ToString(),(int)ManagerLev+1);
        PlayerManager.Self.PlayerReturnLastPoint();
        return DefaultResult;
    }
}
}