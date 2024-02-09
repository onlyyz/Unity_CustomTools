using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("比较本地Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("检查设置变量/比较本地Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconToggleOn), ColorTheme.Type.Red)]
    
    [Serializable]
    public class SetLocalBool : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField] private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();
        public string KeyString;
        public BooleanSet boolset;
        
        //Boolean
        public enum BooleanSet
        {
            Set_false,
            Set_true
        }
        
        protected override string Summary => string.Format(
            "{0} {1}",
            this.KeyString,
            this.boolset switch
            {
                BooleanSet.Set_false => "= false",
                BooleanSet.Set_true => "= true",
                _ => string.Empty
            }
        );
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override bool Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            return (bool)
                   gameObject.GetComponent<LocalNameVariables>().
                Get(KeyString) ==  
                   (BooleanSet.Set_true == boolset) ;
        }
    }
}