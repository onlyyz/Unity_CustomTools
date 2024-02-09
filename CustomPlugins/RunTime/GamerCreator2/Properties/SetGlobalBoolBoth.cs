using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("设置 Bool 切换")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/切换Global Bool状态")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class SetGlobalBoolBoth : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField,GCLabel("Bool名")] private PropertyGetString m_string;
     
        private GlobalNameVariables NameVar;
        public override string Title => $"设置 {this.m_GameObject} bool 切换,从父级获取global";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string boolString = this.m_string.Get(args);
            if (gameObject == null) return DefaultResult;
            
            //TODO:
            // NameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
            
            if (NameVar.Get(boolString) == null)
            {
                Debug.Log("Global Variables 变量 " + gameObject.name + " 不存在，请设置" );
                return DefaultResult;
            }
            NameVar.Set(boolString, !(bool)NameVar.Get(boolString));
          
            return DefaultResult;
        }
    }
}