using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class NearOcclusion : MonoBehaviour
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
    public float Enterlerp = 2.0f;
    [Tooltip("退出缓冲时间")]
    public float Exitlerp = 2.0f;
    [Header("前景遮挡物体")]
    public Renderer[] MeshObj;
    [Header("前景遮挡触发区")]
    public BoxCollider[] TriggerObj;
    
    bool firstEnter;
    float CurrentAlpha;
    float TempAlpha;
    bool isInside;
    int EnterNum;
    //second
    float elapsedTime = 0.0f;
    
    //Gizmos
    [Header("Gizmos")]
    public bool DrawGizmos = true;
    public Color gizmoColor;
    BoxCollider _boxCollider;
    
    //
    static int
        ShaderAplha = Shader.PropertyToID("_TestAlpha");
    
    
    
    private void Awake()
    {
        ResetInit();
        CurrentAlpha = 1.0f;
        EnterNum = 0;

        for (int i = 0; i < TriggerObj.Length; i++)
        {
            if (TriggerObj[i].GetComponent<ChildMessage>() == null)
            {
                TriggerObj[i].gameObject.AddComponent<ChildMessage>();
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.name != "BodyCollider")
    //     {
    //         return;
    //     }
    //     EnterNum++;
    //     if ((Enum == TriggerMode.Single && firstEnter) || Enum == TriggerMode.Multiple)
    //     {
    //         IsInside();
    //         firstEnter = false;
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.name != "BodyCollider")
    //     {
    //         return;
    //     }
    //     //
    //     // Invoke("ExitNumTimer" , 0.5f);
    //     //
    //     if (Enum == TriggerMode.Multiple)
    //     {
    //         Invoke("ExitNumTimer" , 0.25f);
    //     }
    // }
    
    private void TriggerEnterChild(Collider other)
    {
        Debug.Log("Test");
        if (other.name != "BodyCollider")
        {
            return;
        }
        EnterNum++;
        if ((Enum == TriggerMode.Single && firstEnter) || Enum == TriggerMode.Multiple)
        {
            IsInside();
            firstEnter = false;
        }
    }
    
    private void TriggerExitChild(Collider other)
    {
        if (other.name != "BodyCollider")
        {
            return;
        }
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
    
    
    
    #region 携程

    IEnumerator EnterAlphaLerp()
    {
        while (elapsedTime < Enterlerp && isInside)
        {
            float t = elapsedTime / Enterlerp;
            CurrentAlpha = Mathf.Lerp(TempAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            
            //Debug.Log(CurrentAlpha);
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
    
    
    public void CheckAssetDatabase()
    {
        //Check 
        // var Data = Resources.Load<NearOcclusionData>("Textures/texture01");
        //Create
        if (!AssetDatabase.IsValidFolder("Assets/CustomData/Runtime/Foreground Occlusion"))
            AssetDatabase.CreateFolder("Assets/CustomData/Runtime/", "Foreground Occlusion");

        // AssetDatabase.CreateAsset(Data,$"Assets/CustomData/Runtime/Foreground Occlusion/{GoName}.asset");
        AssetDatabase.SaveAssets();      
    }

    

#if UNITY_EDITOR
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
    //     Gizmos.DrawCube(_boxCollider.bounds.center, _boxCollider.bounds.size);
    //     
    // }
#endif
}