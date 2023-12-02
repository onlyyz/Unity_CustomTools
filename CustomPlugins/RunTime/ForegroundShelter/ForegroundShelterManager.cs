using UnityEngine;
using System.Collections;
using GameCreator.Runtime.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif


namespace CustomPlugins.ForegroundShelter
{
    using Data;
    using Signal;
    public class ForegroundShelterManager : MonoBehaviour
    {
        public enum TriggerMode
        {
            Single,
            Multiple
        }
        public TriggerMode Enum;
    
        // public string GoName;
        [Range(0.0f,1.0f)]
        public float targetAlpha;
        [Tooltip("进入缓冲时间")]
        public float Enterlerp = 0.50f;
        [Tooltip("退出缓冲时间")]
        public float Exitlerp = 0.50f;
        [Header("前景遮挡物体")]
        public Renderer[] MeshObj;
        [Header("前景遮挡触发区")]
        public BoxCollider[] TriggerObj;
    
        bool firstEnter = true;
        float CurrentAlpha;
        float TempAlpha;
        bool isInside;
        int EnterNum;
        //second
        float elapsedTime = 0.0f;
    
        static int
            ShaderAplha = Shader.PropertyToID("_Transparency");

        static string _m_tag = "Player";

        private GlobalNameVariables NameVar;
        
        private void Awake()
        {
            ResetInit();
            CurrentAlpha = 1.0f;
            EnterNum = 0;

            for (int i = 0; i < TriggerObj.Length; i++)
            {
                if (TriggerObj[i].GetComponent<CustomMessage>() == null)
                {
                    TriggerObj[i].gameObject.AddComponent<CustomMessage>();
                }
            }
        }


        private void OnEnable()
        {
            NameVar = gameObject.transform.parent.GetComponent<GetGlobalNameData>().NameVar;

            // Debug.Log("OnEnable: " +   NameVar + " Bool:  " + (bool)NameVar.Get(gameObject.name));
            //继续游戏判断
            if ((bool)NameVar.Get(gameObject.name))
            {
                for (int i = 0; i < MeshObj.Length; i++)
                {
                    MeshObj[i].material.SetFloat(ShaderAplha, 0.0f);
                }
            }
        }



        private void TriggerEnterChild(Collider collider)
        {

            if((bool)NameVar.Get(gameObject.name)) return;
            if(!collider.gameObject.tag.Equals(_m_tag)) return;
            // if (!SignalProxy.GetProxyTarget(other.gameObject).HasTag(_m_tag)) return;
            
            // Debug.Log(string.Format( "Enter：<color=#4db8ff>{0} </color>",collider.gameObject.tag));
            
            EnterNum++;
            if ((Enum == TriggerMode.Single && firstEnter) || Enum == TriggerMode.Multiple)
            {
                IsInside();
                
                // Debug.Log("Changer the Variable: " + gameObject.name + " : " + NameVar.Get(gameObject.name));
                if (firstEnter)
                {
                    NameVar.Set(gameObject.name,true);
                    firstEnter = false;
                }
            }
        }
    
        private void TriggerExitChild(Collider collider)
        {
          
            if(!collider.gameObject.tag.Equals(_m_tag)) return;
            // if (!SignalProxy.GetProxyTarget(collider.gameObject).HasTag(_m_tag)) return;
            
            // Debug.Log(string.Format( "Enter：<color=#4db8ff>{0} </color>",collider.gameObject.tag));
            
            if (Enum == TriggerMode.Multiple)
            {
                Invoke("ExitNumTimer" , 0.25f);
            }
        }
        public void ExitNumTimer()
        {
            EnterNum--;
            IsInside();
        }
        public void IsInside()
        {
            elapsedTime = 0.0f;
            TempAlpha = CurrentAlpha;
        
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


        //
        public void BreakDoorCallShelter()
        {
            elapsedTime = 0.0f;
            TempAlpha = CurrentAlpha;
            
            isInside = true;
            
            // Debug.Log("Changer the Variable: " + gameObject.name + " : " + NameVar.Get(gameObject.name));
            if (firstEnter)
            {
                NameVar.Set(gameObject.name,true);
                firstEnter = false;
                
            }
            
            StartCoroutine(EnterAlphaLerp());
        }
        
    
        #region 携程

        IEnumerator EnterAlphaLerp()
        {
            while (elapsedTime < Enterlerp && isInside)
            {
                float t = elapsedTime / Enterlerp;
                CurrentAlpha = Mathf.Lerp(TempAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
            
                
                // Debug.Log(CurrentAlpha);
                for (int i = 0; i < MeshObj.Length; i++)
                {
                    MeshObj[i].material.SetFloat(ShaderAplha,CurrentAlpha);
                }
                yield return null;
            }
        }
    
        IEnumerator ExitAlphaLerp()
        {
            while (elapsedTime < Exitlerp && !isInside)
            {
                float t = elapsedTime / Exitlerp;
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
    
        public void ResetInit()
        {
            firstEnter = true;
        }
    
#if UNITY_EDITOR
        public void CheckAssetDatabase()
        {
            //Check 
            var Data = Resources.Load<ForegroundShelterData>("Textures/texture01");
            //Create
            if (!AssetDatabase.IsValidFolder("Assets/CustomData/Runtime/Foreground Occlusion"))
                AssetDatabase.CreateFolder("Assets/CustomData/Runtime/", "Foreground Occlusion");

            // AssetDatabase.CreateAsset(Data,$"Assets/CustomData/Runtime/Foreground Occlusion/{GoName}.asset");
            AssetDatabase.SaveAssets();      
        }

        /// <summary>
        /// Draws gizmos to show the shape of the zone
        /// </summary>
        // protected virtual void OnDrawGizmos()
        // {
        //    
        //     if (!DrawGizmos)
        //     {
        //         return;
        //     }
        //    
        //     if (_boxCollider==null)
        //     {
        //         _boxCollider = gameObject.GetComponent<BoxCollider>();
        //     }
        //     Gizmos.color = gizmoColor;
        //     Gizmos.DrawWireCube(_boxCollider.bounds.center, _boxCollider.bounds.size);
        //     
        // }
#endif
    }
}