using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("判断父变量 SetActive")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/判断父变量 SetActive")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class ConditionCheckVarySetGOActive : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        GlobalNameVariables NameVar;
        [SerializeField,GCLabel("目标物体")] private PropertyGetGameObject ActiveGo = new PropertyGetGameObject();
        [SerializeField,GCLabel("bool 变量名")] private PropertyGetString m_string = new PropertyGetString();
        
        string boolName;
       
        //Boolean
      
        public override string Title => $"从父物体抓取变量 Set {this.ActiveGo} is {this.m_string}";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            GameObject targetGO = this.ActiveGo.Get(args);
            
            if (gameObject == null) return DefaultResult;
            if (targetGO == null) return DefaultResult;
            
            // NameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
            boolName = this.m_string.Get(args);
            
            
            if (NameVar.Get(boolName) == null)
            {
                // Debug.Log(gameObject.name + "Global Variables 变量不存在，请设置" );
                return DefaultResult;
            }
            targetGO.SetActive((bool)NameVar.Get(boolName));
            
            // Debug.Log("Debug: " + (bool)NameVar.Get(boolName));
            return DefaultResult;
        }
    }
}