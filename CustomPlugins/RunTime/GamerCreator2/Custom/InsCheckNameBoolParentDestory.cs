using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("If self's name bool true 销毁自己")]
    [Description("Destroys a game object scene instance")]
    [Category("逻辑/self's name bool true 销毁自己")]
    [Parameter("YZ", "YZ")]
    [Keywords("Remove", "Delete", "Flush", "MonoBehaviour", "Behaviour", "Script")]
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]
    
    [Serializable]
    public class InsCheckNameBoolParentDestory : TInstructionGameObject
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        private GlobalNameVariables NameVar;
        public override string Title => $"{this.m_GameObject} 名字 bool为true 销毁{this.m_GameObject},变量来自父物体";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            if (gameObject == null) return DefaultResult;
            
            
            NameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;
            if (NameVar.Get(gameObject.name) == null)
            {
                Debug.Log(gameObject.name + " Global Variables 变量未赋值，请设置" );
                return DefaultResult;
            }
            if (!(bool)NameVar.Get(gameObject.name))
                return DefaultResult;
            
            //Destroy
            UnityEngine.Object.Destroy(gameObject);
            return DefaultResult;
        }
    }
}