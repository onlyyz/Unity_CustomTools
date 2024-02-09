using System;
using System.Threading.Tasks;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("Get Global Set Local Bool")]
    [Description("Destroys a game object scene instance")]
    [Category("赋值/Get Global set Local Bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class GetAllBoolBoth : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField,GCLabel("全局 Bool名")] private PropertyGetString m_Globalstring;
        [SerializeField,GCLabel("本地 Bool名")] private PropertyGetString m_Localstring;
        GlobalNameVariables GlobalNameVar;
        public override string Title => $"GL {this.m_Globalstring} Set {this.m_Localstring} bool";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            string globalString = this.m_Globalstring.Get(args);
            string localString = this.m_Localstring.Get(args);
            if (gameObject == null) return DefaultResult;
            
            //TODO:
            // GlobalNameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
         
            gameObject.GetComponent<LocalNameVariables>().Set(localString, (bool)GlobalNameVar.Get(globalString));
            
            return DefaultResult;
        }
    }
}