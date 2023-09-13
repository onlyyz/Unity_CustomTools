 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    PostFXSettings settings;
    CustomRenderPass PostPass = null;
    
    public override void Create()
    {
        if(settings==null)
            return;
        if(settings.shader==null)
            return;
        if (settings.Blurshader == null)
        {
            Debug.Log("Blur Shader Null");
            return;
        }
            
            
        PostPass = new CustomRenderPass(settings);
        PostPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;   
    }
  
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        PostPass.ConfigureInput(ScriptableRenderPassInput.Color);
        PostPass.ConfigureInput(ScriptableRenderPassInput.Depth);
        PostPass.SetRenderTarget(renderer.cameraColorTargetHandle);
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(PostPass);
    }
    
    protected override void Dispose(bool disposing)
    {
        PostPass.Dispose();
    }
}