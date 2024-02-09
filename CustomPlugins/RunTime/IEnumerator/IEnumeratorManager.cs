using System;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.VFX;

namespace CustomPlugins.Manager
{
    public class IEnumeratorManager : ManagerBase<IEnumeratorManager>
    {
        public const float 
            TimeScale = 3.0f,
            PlayerHigh = 1.5f;
        private Vector3 IEPlayerPos;
      
        static string
            VFXPosition = "ConformCenter",
            VFXLifeTime = "LifeTime",
            
            
            //Shader To ID
            shaderAplha_ID = "_Transparency";

        static int
            ShaderAplha = Shader.PropertyToID(shaderAplha_ID);
        
        private bool LocalName;
        private void Awake()
        {
            Self = this;
        }

        public float GetTimeScale()
        {
            return TimeScale;
        }
        public void SetLocalName(bool boolValue)
        {
            LocalName = boolValue;
        }

        #region VFX IE
      
        public void EnemyDiedPlayerAbsorb(VisualEffect instance,float time)
        {
            StartCoroutine(IEEnemyDiedPlayerAbsorb(instance,time));
        }

        public void EnemyDiedPlayerAbsorb(Args args,FieldSetLocalName Variable,VisualEffect instance,float time)
        {
          
            StartCoroutine(IEEnemyDiedPlayerAbsorb(args,instance,time,true));
        }
        IEnumerator IEEnemyDiedPlayerAbsorb(VisualEffect instance,float time)
        {
            //正在解锁
            float timeout = Time.time + time * TimeScale;
            int tempCount = 0;
            
            while (Time.time < timeout)
            {
                instance.SetVector3(VFXPosition,PlayerManager.Self.GetPlayPostion()+ new Vector3(0.0f,PlayerHigh/2,0.0f));
                var effectCount = instance.aliveParticleCount;

                if (instance && effectCount != 0)
                {
                    if (tempCount == 0 || tempCount<=effectCount)
                    {
                        tempCount = effectCount;
                        // Debug.Log("Win: "  + effectCount);
                    }
                    else
                    {
                        // Debug.Log("lose: "  + effectCount);
                        Destroy(instance.gameObject);
                        yield break;
                    }
                }
                
                yield return null;
            }
            Destroy(instance.gameObject);
        }
        
        IEnumerator IEEnemyDiedPlayerAbsorb(Args args,VisualEffect instance,float time,bool SetBool = false)
        {
            //正在解锁
            float timeout = Time.time + time * TimeScale;
            int tempCount = 0;
            
            while (Time.time < timeout)
            {
                instance.SetVector3(VFXPosition,PlayerManager.Self.GetPlayPostion()+ new Vector3(0.0f,PlayerHigh/2,0.0f));
                var effectCount = instance.aliveParticleCount;

                if (instance && effectCount != 0)
                {
                    if (tempCount == 0 || tempCount<=effectCount)
                    {
                        Debug.Log("Win: "  + effectCount);
                        tempCount = effectCount;
                    }
                    else
                    {
                        Debug.Log("lose: "  + effectCount);
                        if (SetBool)
                        {
                           
                        }
                        Destroy(instance.gameObject);
                        yield break;
                    }
                }
                
                yield return null;
            }
            Destroy(instance.gameObject);
        }
        
        public bool EnemyDiedPlayerWaitForAbsorbExtendTimeDestroy(Args args,VisualEffect instance,float time)
        {
                // 启动协程，并传递回调函数
            StartCoroutine(IEEnemyDiedPlayerAbsorbExtendTimeDestroy(args, instance, time, callback =>
            {
                Debug.Log("Result in HandleCoroutineResult: " + callback);
            }));
            // 返回协程的结果
            return false;
        }
        
        IEnumerator IEEnemyDiedPlayerAbsorbExtendTimeDestroy(Args args,VisualEffect instance,float time,Action<bool> callback)
        {
            //正在解锁
            float timeout = Time.time + time * TimeScale;
            int tempCount = 0;
            
            while (Time.time < timeout)
            {
                instance.SetVector3(VFXPosition,PlayerManager.Self.GetPlayPostion()+ new Vector3(0.0f,PlayerHigh/2,0.0f));
                var effectCount = instance.aliveParticleCount;

                if (instance && effectCount != 0)
                {
                    if (tempCount == 0 || tempCount<=effectCount)
                    {
                        Debug.Log("Win: "  + effectCount);
                        tempCount = effectCount;
                    }
                    else
                    {
                        Debug.Log("lose: "  + effectCount);
                        
                        // 在协程结束时调用回调，并传递布尔值
                        callback?.Invoke(true);
                        Destroy(instance.gameObject);
                        yield break;
                    }
                }
                
                yield return null;
            }
            Destroy(instance.gameObject);
        }
        
        #endregion


        #region 透明度平滑

        public void ShelterAlphaLerp(Args args, FieldSetLocalName variable ,GameObject[] gameObjects, float lerpTime, bool isInside)
        {

            StartCoroutine(TriggerAlphaLerp(
                (float)(double)variable.Get(args),
                isInside ? 0.0f : 1.0f,
                lerpTime, isInside, gameObjects,
                result =>
                {
                    variable.Set(result, args);
                }));
        }
            
        IEnumerator TriggerAlphaLerp(float currentAlpha,float targetAlpha, float lerpTime,bool isInside,GameObject[] gameObjects,Action<float> callback)
        {
            float elapsedTime = 0.0f;
            float tempAlpha = currentAlpha;
            
            while (elapsedTime < lerpTime)
            {
                if (currentAlpha < 0.25f && isInside)
                {
                    currentAlpha = 0.0f;
                }
                else if(currentAlpha > 0.75f && !isInside)
                {
                    currentAlpha = 1.0f;
                }
                else
                {
                     currentAlpha =(Mathf.Lerp(tempAlpha, targetAlpha, elapsedTime / lerpTime));
                }
               
                elapsedTime += Time.deltaTime;
                
                foreach (var obj in gameObjects)
                {
                   if(obj ==null)
                       continue;
                   obj.GetComponent<Renderer>().material.SetFloat(ShaderAplha,currentAlpha);
                }

                callback?.Invoke(Mathf.Clamp01(currentAlpha));
                yield return null;
            }
        }

        #endregion
        
        //=               Debug                      =
        //=============================================
        public void DebugLog(string str)
        {
            Debug.Log(str);
        }
        //=============================================
    }
}