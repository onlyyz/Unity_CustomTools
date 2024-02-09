using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Common.Title("设置Fluid ++ --")]
    [Description("Add two values together")]
    [Category("检查设置变量/设置Fluid ++ --")]
    
    [Keywords("Sum", "Plus", "Float", "Integer", "Variable")]
    [Image(typeof(IconPlusCircle), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InsFluidAdd : InstructionTraits
    {
        [GCLabel("运算方式")] public Math math;
       
        public enum Math
        {
            Add,
            Sub
        }

        public override string Title  => string.Format(
            "运算方式为{0}",
            this.math
        );
        protected override Task Run(Args args)
        {
            int fluid = this.m_Traits.GetFluid(args);
            if (math==Math.Add)
            {
                this.m_Traits.SetFluid(args,fluid + 1);
            }
            else
            {
                this.m_Traits.SetFluid( args,fluid - 1);
            }
           
            return DefaultResult;
        }
    }
}