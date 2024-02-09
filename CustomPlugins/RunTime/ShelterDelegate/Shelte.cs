using System;
using System.Collections;
using GameCreator.Runtime.Variables;
using Sirenix.OdinInspector;
using UnityEngine;


namespace CustomPlugins.ForegroundShelter
{
    public enum TriggerMode
    {
        Single,
        Multiple
    }
    public class Shelte : MonoBehaviour
    {
         
        #region 面板
        [LabelText("触发模式")]
        public TriggerMode Enum;
        [LabelText("目标透明度")]public float targetAlpha;
        [LabelText("渐变过渡时间"),Range(0.0f,1.5f)]public float Lerptime;
       
        [LabelText("存储数据")]
        public LocalNameVariables SatesData;
      
        [LabelText("前景遮挡物体")]
        public Renderer[] MeshObj;
        [LabelText("前景遮挡触发区")]
        public BoxCollider[] TriggerObj;
        
        
        int EnterNum;
        bool isInside;
      
        float CurrentAlpha;
        float TempAlpha;
        float elapsedTime;
        
        private static string
            nv_KeyString = "KeyString",
            nv_lerpTime = "Lerptime",
            nv_used = "Obj_Used",

            //Shader
            shader_ID = "_Transparency";
        
        
        
        static int
            ShaderAplha = Shader.PropertyToID(shader_ID);

        #endregion
        
        #region  初始化
        
        // private void OnEnable()
        // {
        //     Initialization();
        // }
        public void Initialization()
        {
            
            CurrentAlpha = 1.0f;
            EnterNum = 0;
            targetAlpha = 0.0f;
            
            // 继续游戏判断
            if (Enum == TriggerMode.Single)
            {
                for (int i = 0; i < MeshObj.Length; i++)
                {
                    MeshObj[i].material.SetFloat(ShaderAplha, (bool)SatesData.Get(nv_used)?0.0f:1.0f);
                }
            }

            if (Enum == TriggerMode.Multiple || !(bool)SatesData.Get(nv_used))
            {
                for (int i = 0; i < TriggerObj.Length; i++)
                {
                    if (TriggerObj[i].gameObject.GetComponent<ShelterDelegate>() == null)
                    {
                        TriggerObj[i].gameObject.AddComponent<ShelterDelegate>().
                            SetKeyString(this);
                    }
                }
            }
        }
        #endregion
        
        #region 触发逻辑
       
        public void CompareStrings(bool isEnter)
        {
            //单次已触发
          if(Enum == TriggerMode.Single && (bool)SatesData.Get(nv_used))
              return;
            
          if (isEnter)
            {
                // Debug.Log("进入逻辑");
                TriggerEnterChild();
            }   
            else
            {
                // Debug.Log("退出逻辑");
                TriggerExitChild();
            }
        }
        
        private void TriggerEnterChild()
        {
            EnterNum++;
            IsInside();
            SatesData.Set(nv_used,true);
        }
        private void TriggerExitChild()
        {
          
            if (Enum == TriggerMode.Multiple)
            {
                Invoke("ExitNumTimer" , 0.1f);
            }
        }
        public void ExitNumTimer()
        {
            EnterNum--;
            IsInside();
        }
        private void IsInside()
        {
            if (EnterNum > 0)
            {
                isInside = true;
                StartCoroutine(EnterAlphaLerp());
                // Debug.Log("里面: " + isInside);
            }
            else
            {
                isInside = false;
                StartCoroutine(ExitAlphaLerp());
                // Debug.Log("外面: " + isInside);
            }
        }
        #endregion

        #region 广播触发
        public void BreakDoorCallShelter()
        {
            Enum = TriggerMode.Single;
            
            elapsedTime = 0.0f;
            TempAlpha = CurrentAlpha;
            isInside = true;
            
            SatesData.Set(nv_used,true);
            StartCoroutine(EnterAlphaLerp());
        }
        #endregion
    
        #region Time IEnumerator

        IEnumerator EnterAlphaLerp()
        {
            elapsedTime = 0.0f;
            TempAlpha = CurrentAlpha;
            while (elapsedTime < Lerptime && isInside)
            {
                float t = elapsedTime / Lerptime;
                CurrentAlpha = Mathf.Lerp(TempAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
                
                for (int i = 0; i < MeshObj.Length; i++)
                {
                    MeshObj[i].material.SetFloat(ShaderAplha,CurrentAlpha);
                }
                yield return null;
            }
        }
    
        IEnumerator ExitAlphaLerp()
        {
            elapsedTime = 0.0f;
            TempAlpha = CurrentAlpha;
            while (elapsedTime < Lerptime && !isInside)
            {
                float t = elapsedTime / Lerptime;
                CurrentAlpha = Mathf.Lerp(TempAlpha,1.0f, t);
                elapsedTime += Time.deltaTime;
            
            
                // Debug.Log(CurrentAlpha);
                for (int i = 0; i < MeshObj.Length; i++)
                {
                    MeshObj[i].material.SetFloat(ShaderAplha,CurrentAlpha);
                }
                yield return null;
            }
        }
        #endregion
    }
}