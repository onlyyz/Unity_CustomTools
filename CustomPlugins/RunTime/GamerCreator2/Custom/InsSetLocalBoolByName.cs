using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("Set local Bool By name")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/Set Local bool By name")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
   
    
    [Serializable]
    public class InsSetLocalBoolByName : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField,GCLabel("本地变量名")] public PropertyGetString m_string;
        [SerializeField] bool setBool;
        public override string Title => $"设置 {this.m_string} 为 {this.setBool}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string boolString = this.m_string.Get(args);
            if (gameObject == null) return DefaultResult;
            
            gameObject.GetComponent<LocalNameVariables>().Set(boolString,setBool);
            
            return DefaultResult;
        }
    }
}