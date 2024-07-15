using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        IEnumerator TriggerAlphaLerp(float currentAlpha,float targetAlpha, float lerpTime,bool isInside,GameObject[] gameObjects ,Action<float> callback)
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


        #region Destroy
        public void ObjectDestroy(float time, GameObject gameObject)
        {
            StartCoroutine(IEObjectDestroy(time, gameObject));
        }

        private IEnumerator IEObjectDestroy(float time, GameObject gameObject)
        {
            float startTime = Time.time;
            while (Time.time - startTime < time)
                yield return null;
            Destroy(gameObject);
        }
        #endregion

        #region 震动
        public void StartRumble(float duration, float intensity)
        {
            HandleRumble(intensity);
            StartCoroutine(StopRumbleAfterDelay(duration));
        }
        public void CurveRumble(float duration, AnimationCurve intensityCurve)
            => StartCoroutine(RumbleCoroutine(duration,intensityCurve));
        private IEnumerator RumbleCoroutine(float duration, AnimationCurve intensityCurve)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float currentIntensity = intensityCurve.Evaluate(elapsed / duration);
                HandleRumble(currentIntensity);
                
                yield return null;
                elapsed += Time.deltaTime;
            }
            HandleRumble();
        }
        private IEnumerator StopRumbleAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            HandleRumble();
        }

        private void HandleRumble(float value = 0)
        {
            var gamepads = Gamepad.all;
            if (gamepads.Count == 0)
            {
                Debug.LogWarning("No gamepads connected.");
                return;
            }
            
            foreach (var gamepad in Gamepad.all)
            {
                gamepad.SetMotorSpeeds(value, value);
            }
        }
        #endregion
    }
}