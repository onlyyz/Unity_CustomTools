using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPlugins.Manager;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;



namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 0, 1)]
    
    [Title("透明度过渡")]
    [Description("Destroys a game object scene instance")]
    [Category("渲染_VFX/透明度过渡")]
    [Parameter("YZ", "YZ")]
   
    [Image(typeof(IconColor), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class IEnumeratorLerpAlpha : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField, GCLabel("InterObject")] private PropertyGetGameObject m_interObj;
        [SerializeField, GCLabel("里面外面")] private PropertyGetBool inSide;
        [SerializeField, GCLabel("过渡时间")] private PropertyGetDecimal lerpTime;
        
        [SerializeField, Header("当前透明度")]
        protected FieldSetLocalName variable = new FieldSetLocalName(ValueNumber.TYPE_ID);
        [SerializeField, Header("模型组")]
        protected LocalListVariables listVariable;

        // RUN METHOD: ----------------------------------------------------------------------------

        public override string Title => $"设置模型Alpha 在 {this.lerpTime.ToString()} " +
                                        $"后过渡为 {(this.inSide.ToString() =="true" ? "0" :"1")}";

        protected override async Task Run(Args args)
        {
            InterObject interObject = this.m_interObj.Get(args).GetComponent<InterObject>();
            GameObject[] meshObj;
            
            var isInside = this.inSide.Get(args);
            var alphaLerpTime = (float)this.lerpTime.Get(args);
            
            if (interObject.GetGameObjectCount() != 0)
            {
                IEnumeratorManager.Self.ShelterAlphaLerp(args, variable,interObject.GetGameObjects(), alphaLerpTime, isInside);
            }
            else
            {
                meshObj = new GameObject[listVariable.Count];
                for (int i = 0; i < listVariable.Count; i++)
                {
                     meshObj[i] = (GameObject)listVariable.Get(i);
                }
                IEnumeratorManager.Self.ShelterAlphaLerp(args, variable,meshObj, alphaLerpTime, isInside);
            }
            
            
           
           
        }
    }
}