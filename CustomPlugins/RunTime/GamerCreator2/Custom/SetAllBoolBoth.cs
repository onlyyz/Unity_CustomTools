using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("Set All Bool 切换")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/切换All Bool状态")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class SetAllBoolBoth : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("全局 Bool名")] private PropertyGetString m_Globalstring;
        [SerializeField,GCLabel("本地 Bool名")] private PropertyGetString m_Localstring;
        GlobalNameVariables GlobalNameVar;
        LocalNameVariables LocalNameVar;
        public override string Title => $"Set All bool 切换, 父级global Self's Local";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string globalString = this.m_Globalstring.Get(args);
            string localString = this.m_Localstring.Get(args);
            
            if (gameObject == null) return DefaultResult;
          
            GlobalNameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
            LocalNameVar = gameObject.GetComponent<LocalNameVariables>();

            var boolSet = (bool)GlobalNameVar.Get(globalString);
            GlobalNameVar.Set(globalString, !boolSet);
            LocalNameVar.Set(localString, !boolSet);
        
            return DefaultResult;
        }
    }
}