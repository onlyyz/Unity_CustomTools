using System;
using System.Threading.Tasks;
using CustomPlugins.Manager;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("VFX后等待")]
    [Description("Waits a certain amount of seconds")]

    [Category("渲染_VFX/VFX后等待")]

    [Parameter(
        "Seconds",
        "The amount of seconds to wait"
    )]
    
    [Parameter(
        "Mode",
        "Whether to use the time scale or not"
    )]

    [Keywords("Wait", "Time", "Seconds", "Minutes", "Cooldown", "Timeout", "Yield")]
    [Image(typeof(IconTimer), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class VFXWaitSecondsMulTimeScale : Instruction
    {
        [SerializeField]
        private PropertyGetDecimal m_Seconds = new PropertyGetDecimal(1f);

        [SerializeField]
        private TimeMode m_Mode = new TimeMode(TimeMode.UpdateMode.GameTime);

        public override string Title =>
            $"调用VFX后 等待 {this.m_Seconds} {(this.m_Seconds.ToString() == "1" ? "S " : " S ")}调用后续函数";

        protected override async Task Run(Args args)
        {
            float value = (float) this.m_Seconds.Get(args);
            await this.Time(
                value * IEnumeratorManager.Self.GetTimeScale()
                , this.m_Mode
                );
        }
    }
}