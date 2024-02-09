using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Common.Title("设置本地Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/设置本地Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class CompareLocolBool : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [LabelText("Key")]
        public string KeyString;
        [LabelText("设置状态")]
        public BooleanSet boolset;
        
        //Boolean
        public enum BooleanSet
        {
            Set_false,
            Set_true
        }
        
        public override string Title => $"设置 {this.m_GameObject} 本地变量{this.KeyString}";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            gameObject.GetComponent<LocalNameVariables>().
                Set(
                    KeyString,
                    BooleanSet.Set_true == boolset
                    );
            return DefaultResult;
        }
    }
}