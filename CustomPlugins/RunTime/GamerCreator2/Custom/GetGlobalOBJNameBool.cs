using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("设置 Bool for GO's name")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/Set GO's name bool")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    // [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class GetGlobalOBJNameBool : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        private GlobalNameVariables NameVar;

        public BooleanSet boolset;
        
        //Boolean
        public enum BooleanSet
        {
            Set_false,
            Set_true
        }
        
        public override string Title => $"抓取 {this.m_GameObject} for Global Variables";
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            if (gameObject == null) return DefaultResult;
            
            NameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
            if (NameVar.Get(gameObject.name) == null)
            {
                Debug.Log("Global Variables 变量 " + gameObject.name + " 不存在，请设置" );
                return DefaultResult;
            }
            
            NameVar.Set(gameObject.name, (boolset == BooleanSet.Set_true));
            // Debug.Log("设置变量： " + NameVar.Get(gameObject.name));
            return DefaultResult;
        }
    }
}