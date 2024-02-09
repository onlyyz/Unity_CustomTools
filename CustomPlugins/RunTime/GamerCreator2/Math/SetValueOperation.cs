using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Common.Title("设置值 ++ --")]
    [Description("Add two values together")]
    [Category("检查设置变量/设置值 ++ --")]
    
    [Keywords("Sum", "Plus", "Float", "Integer", "Variable")]
    [Image(typeof(IconPlusCircle), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class SetValueOperation : Instruction
    {
        [GCLabel("运算方式")] public Math math;
        [SerializeField] 
        private PropertySetNumber m_Set = SetNumberLocalName.Create;
        public enum Math
        {
            Add,
            Sub
        }
        public override string Title =>
            $"{this.m_Set}{(this.math == Math.Add ? "++" : "--")}";
        protected override Task Run(Args args)
        {
            var value = (int)this.m_Set.Get(args);
            
            if (math==Math.Add)
            {
                this.m_Set.Set(value + 1, args);
            }
            else
            {
                this.m_Set.Set(value - 1, args);
            }
           
            return DefaultResult;
        }
    }
}