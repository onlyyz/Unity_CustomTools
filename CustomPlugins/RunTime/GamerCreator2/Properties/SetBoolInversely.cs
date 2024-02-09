using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]


    [Common.Title("设置Bool为相反")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/设置Bool为相反")]
    [Parameter("YZ", "YZ")]
    [Image(typeof(IconZoom), ColorTheme.Type.Green)]

    [Serializable]
    public class SetBoolInversely : TInstructionGameObject
    {
        [LabelText("Key")] public string KeyString;
        public override string Title => $"设置 {this.KeyString} 为相反变量";
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            var nv = gameObject.GetComponent<LocalNameVariables>();
            var SetBool = (bool)nv.Get(KeyString);
            
            nv.Set(
                KeyString,
                !SetBool
            );
            return DefaultResult;
        }
    }
}