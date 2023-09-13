using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal.Internal;
using ProfilingScope = UnityEngine.Rendering.ProfilingScope;
using static PostFXSettings;

public class CustomRenderPass : ScriptableRenderPass
{
    enum Pass {
        Copy,
        BloomCombine,
        // BloomAdd,
        // BloomHorizontal,
        // BloomPrefilter,
        BloomPrefilterFireflies,
        // BloomScatter,
        // BloomScatterFinal,
        // BloomVertical,
        ColorGradingNone,
        ColorGradingACES,
        ColorGradingNeutral,
        ColorGradingReinhard,
        ApplyColorGrading,
        ApplyColorGradingWithLuma,
        FinalRescale,
        // FXAA,
        // FXAAWithLuma
    }
    Pass pass;
    string passName;
    PostFXSettings settings;
    
    static readonly string cmd_RenderTag = "Custom Render Pass";

    //Bloom
    int
        bloomBucibicUpsamplingId = Shader.PropertyToID("_BloomBicubicUpsampling"),
        bloomPrefilterId = Shader.PropertyToID("_BloomPrefilter"),
        bloomThresholdId = Shader.PropertyToID("_BloomThreshold"),
        fxSourceId = Shader.PropertyToID("_PostFXSource"),
        fxSource2Id = Shader.PropertyToID("_PostFXSource2");

    //Color Adjust
    int
        colorGradingLUTId = Shader.PropertyToID("_ColorGradingLUT"),
        colorGradingLUTParametersId = Shader.PropertyToID("_ColorGradingLUTParameters"),
        colorGradingLUTInLogId = Shader.PropertyToID("_ColorGradingLUTInLogC"),
        colorAdjustmentsId = Shader.PropertyToID("_ColorAdjustments"),
        colorFilterId = Shader.PropertyToID("_ColorFilter"),
        whiteBalanceId = Shader.PropertyToID("_WhiteBalance"),
        splitToningShadowsId = Shader.PropertyToID("_SplitToningShadows"),
        splitToningHighlightsId = Shader.PropertyToID("_SplitToningHighlights"),
        channelMixerRedId = Shader.PropertyToID("_ChannelMixerRed"),
        channelMixerGreenId = Shader.PropertyToID("_ChannelMixerGreen"),
        channelMixerBlueId = Shader.PropertyToID("_ChannelMixerBlue"),
        smhShadowsId = Shader.PropertyToID("_SMHShadows"),
        smhMidtonesId = Shader.PropertyToID("_SMHMidtones"),
        smhHighlightsId = Shader.PropertyToID("_SMHHighlights"),
        smhRangeId = Shader.PropertyToID("_SMHRange");
    
    int colorLUTResolution;
    bool useHDR;
    
    CommandBuffer cmd = default;
    
    Material material;
    Material blurmaterial;
    
    RTHandle scoureId;
    RTHandle destinId;
    RTHandle bloomFinalId;

    bool IsActive;
    	
    
    //Bloom
    struct Level
    {
        internal int[] _BloomMipDown;
        internal int[] _BloomMipUp;
    }
    const int maxBloomPyramidLevels = 16;
    readonly GraphicsFormat m_DefaultHDRFormat;
    // [down,up]
    Level m_Pyramid;
    internal RTHandle[] m_BloomMipUp;
    internal RTHandle[] m_BloomMipDown;
    
   
    public CustomRenderPass(PostFXSettings postSettings)
    {
        this.settings = postSettings;
        this.material =  CoreUtils.CreateEngineMaterial(settings.shader);
        this.blurmaterial = CoreUtils.CreateEngineMaterial(settings.Blurshader);

        
        m_Pyramid._BloomMipUp = new int[maxBloomPyramidLevels];
        m_Pyramid._BloomMipDown = new int[maxBloomPyramidLevels];
        m_BloomMipUp = new RTHandle[maxBloomPyramidLevels];
        m_BloomMipDown = new RTHandle[maxBloomPyramidLevels];
        
        for (int i = 0; i < maxBloomPyramidLevels; i++)
        {
            m_Pyramid._BloomMipDown[i] = Shader.PropertyToID("_BloomMipUp" + i);
            m_Pyramid._BloomMipUp[i] = Shader.PropertyToID("_BloomMipDown" + i);
            // Get name, will get Allocated with descriptor later
            m_BloomMipUp[i] = RTHandles.Alloc(m_Pyramid._BloomMipUp[i], name: "_BloomMipUp" + i);
            m_BloomMipDown[i] = RTHandles.Alloc(m_Pyramid._BloomMipDown[i], name: "_BloomMipDown" + i);
        }
    }
    
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        
        // Configures render targets for this render pass. Call this instead of CommandBuffer.SetRenderTarget.
        //This method should be called inside Configure.
        ConfigureTarget(this.scoureId);
        useHDR = renderingData.cameraData.isHdrEnabled;
    }
    public void SetRenderTarget(RTHandle soure)
    {
        this.scoureId = soure;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var stack = VolumeManager.instance.stack;
        cmd = CommandBufferPool.Get(cmd_RenderTag);
        
        Render(cmd, ref renderingData);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ref var cameraData = ref renderingData.cameraData;
        IsActive = cameraData.cameraType <= CameraType.SceneView ? true : false;
      
        
        
        using (new ProfilingScope(cmd, new ProfilingSampler("Post FX")))
        {
            // DoBloom(ref renderingData);
            // Draw(bloomFinalId, scoureId,  Pass.Copy);
            DoFinal(ref renderingData,scoureId);
        
            // Draw(scoureId, scoureId,  (int)Blur.pass);
            // DoFinal(ref renderingData);
            
            // Debug.Log(DoBloom(ref renderingData));
            
            // if (DoBloom(ref renderingData)) {
            // Debug.Log("Boom");
            // DoFinal(ref renderingData,bloomFinalId);
            //     // buffer.ReleaseTemporaryRT(bloomResultId);
            //     
            // }
            // else {
            //     Debug.Log(" not Boom");
            //     DoFinal(ref renderingData,scoureId);
            // }
        }
    }

    bool DoBloom (ref RenderingData renderingData) {
        
        var bloom = settings.Bloom;
        var Blur = settings.Blur;
        ref var cameraData = ref renderingData.cameraData;
        // cameraData.isHdrEnabled
        var desc = cameraData.cameraTargetDescriptor;
        
        //RT  use RT desc to get common desc
        desc.msaaSamples = 1;
        desc.depthBufferBits = 0;
       
        //don't bloom
        if (Blur.maxIterations == 0 ||Blur.intensity <= 0f||// bloom.intensity <= 0f||
            desc.height < Blur.downscaleLimit * 2 || desc.width < Blur.downscaleLimit * 2 ) {
            return false;
        }
        
        cmd.BeginSample("Bloom");
        //threshold caculate
        #region threshold
        Vector4 threshold;
        threshold.x = Mathf.GammaToLinearSpace(bloom.threshold);
        threshold.y = threshold.x * bloom.thresholdKnee;
        threshold.z = 2f * threshold.y;
        threshold.w = 0.25f / (threshold.y + 0.00001f);
        threshold.y -= threshold.x;
        cmd.SetGlobalVector(bloomThresholdId, threshold);
        #endregion
        
        var templaId = scoureId;
        
        
        
        //ToDo:待优化
        passName = Enum.GetName(typeof(PostFXSettings.BlurSettings.PassEnum), Blur.Pass );
        cmd.BeginSample(passName);


        var OnePass = (passName == "Box" || passName == "Kawase");
        int TwoPass =  OnePass ? (int)Blur.Pass : (int)Blur.Pass + 1;
        
       
        if (Blur.Pass == 0)
        {
            Debug.Log("Don't BLur");
            return false;
        }
        
        
        bloomFinalId = RTHandles.Alloc(desc);
        destinId = RTHandles.Alloc(desc);
        if (OnePass&&!Blur.upDownSample)
        {
            // var RT = templaId;
            var RT = RTHandles.Alloc(desc);
            for (int i =0; i < Blur.maxIterations; i++)
            {
                //To Screen
                // Draw(templaId,RT,(int)Blur.Pass);
                // Draw(RT,templaId,(int)Blur.Pass);
                
                
                Draw(templaId,RT,(int)Blur.Pass);
                Draw(RT,destinId,(int)Blur.Pass);
                
                templaId = destinId;
            }
            
            // Draw(templaId,templaId,(int)Blur.Pass);
            // Debug.Log("One Pass: " + passName);
        }
        else 
        {
            int i;
            
            // Debug.Log("Two Pass: " + passName);
            for (i =0; i < Blur.maxIterations; i++)
            {
                if (desc.width == 1||desc.height == 1) 
                    break;
                RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipUp[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: m_BloomMipUp[i].name);
                RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipDown[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp,name: m_BloomMipDown[i].name);
                //Horizontal
                Draw(templaId, m_BloomMipUp[i], (int)Blur.Pass);
                //Vertical
                Draw(m_BloomMipUp[i], m_BloomMipDown[i], TwoPass);
                templaId = m_BloomMipDown[i];
                
                desc.width = Mathf.Max(1, desc.width >> 1);
                desc.height = Mathf.Max(1, desc.height >> 1);
            }
            
            cmd.EndSample(passName);
            cmd.SetGlobalFloat(
                bloomBucibicUpsamplingId, bloom.bicubicUpsampling ? 1f : 0f
            );
            
            templaId = m_BloomMipDown[i - 1];
            for (i -= 2; i > 0; i--) 
            {
                var mipUp = m_BloomMipDown[i];
                cmd.SetGlobalTexture(fxSource2Id, mipUp);
                Draw(templaId, mipUp, Pass.BloomCombine);
                templaId = mipUp;
            }
        }
        
        // cmd.SetGlobalTexture(fxSource2Id, scoureId);
        // Draw(templaId, scoureId, Pass.Copy);
        // Draw(templaId, scoureId, Pass.BloomCombine);
        
        
        //Bloom
        Draw(templaId, bloomFinalId, Pass.Copy);
        // Draw(bloomFinalId, scoureId, Pass.Copy);
        
        
        cmd.SetGlobalFloat("_BlurRange", Blur.BlurRange);
        cmd.EndSample("Bloom");
        
        return true;
    }
    
    
    #region Color Adjust and the Tone Mapping
    //Tone Mapping
    void ConfigureColorAdjustments () {
        ColorAdjustmentsSettings colorAdjustments = settings.ColorAdjustments;
        //曝光、对比度、色调偏移和饱和度
        cmd.SetGlobalVector(colorAdjustmentsId, new Vector4(
            Mathf.Pow(2f, colorAdjustments.postExposure),
            colorAdjustments.contrast * 0.01f + 1f,
            colorAdjustments.hueShift * (1f / 360f),
            colorAdjustments.saturation * 0.01f + 1f
        ));
        cmd.SetGlobalColor(colorFilterId, colorAdjustments.colorFilter.linear);
    }
    //白平衡
    void ConfigureWhiteBalance () {
        WhiteBalanceSettings whiteBalance = settings.WhiteBalance;
        //色温、色调
        cmd.SetGlobalVector(whiteBalanceId, ColorUtils.ColorBalanceToLMSCoeffs(
            whiteBalance.temperature, whiteBalance.tint
        ));
    }
    //色调分割 伽马空间
    void ConfigureSplitToning () {
        SplitToningSettings splitToning = settings.SplitToning;
        Color splitColor = splitToning.shadows;
        //平衡值
        splitColor.a = splitToning.balance * 0.01f;
        cmd.SetGlobalColor(splitToningShadowsId, splitColor);
        cmd.SetGlobalColor(splitToningHighlightsId, splitToning.highlights);
    }
    //通道混合器 行是输出的颜色，XYZ列是RGB输入
    void ConfigureChannelMixer () {
        ChannelMixerSettings channelMixer = settings.ChannelMixer;
        cmd.SetGlobalVector(channelMixerRedId, channelMixer.red);
        cmd.SetGlobalVector(channelMixerGreenId, channelMixer.green);
        cmd.SetGlobalVector(channelMixerBlueId, channelMixer.blue);
    }
    // 阴影、中值、高光
    void ConfigureShadowsMidtonesHighlights () {
        ShadowsMidtonesHighlightsSettings smh = settings.ShadowsMidtonesHighlights;
        cmd.SetGlobalColor(smhShadowsId, smh.shadows.linear);
        cmd.SetGlobalColor(smhMidtonesId, smh.midtones.linear);
        cmd.SetGlobalColor(smhHighlightsId, smh.highlights.linear);
        cmd.SetGlobalVector(smhRangeId, new Vector4(
            smh.shadowsStart, smh.shadowsEnd, smh.highlightsStart, smh.highLightsEnd
        ));
    }
    #endregion
    

    //做的不仅仅是调色还有色调映射
    //FXAA.enable > 调色 color grading > 应用FXAA
    
    void DoFinal(ref RenderingData renderingData, RTHandle source)
    {
        #region Configure Color Adjust and the Tone Mapping
        //曝光、对比度、色调偏移和饱和度
        ConfigureColorAdjustments();
        //白平衡   色温、色调
        ConfigureWhiteBalance();
        //色调分割 伽马空间
        ConfigureSplitToning();
        //通道混合器 行是输出的颜色，XYZ列是RGB输入
        ConfigureChannelMixer();
        // 阴影、中间色调、高光
        ConfigureShadowsMidtonesHighlights();
        #endregion
        
        ref var postProcessingData = ref renderingData.postProcessingData;
        bool hdr = postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
        
        int lutHeight = postProcessingData.lutSize;
        int lutWidth = lutHeight * lutHeight;
        
       
        // var format = hdr ? m_HdrLutFormat : m_LdrLutFormat;
        // var descriptor = new RenderTextureDescriptor(lutWidth, lutHeight, format, 0);
        // cmd.GetTemporaryRT(
        //     colorGradingLUTId, lutWidth, lutHeight, 0,
        //     FilterMode.Bilinear, RenderTextureFormat.DefaultHDR
        // );
        
        // internalColorLut = UniversalRenderer.CreateRenderGraphTexture(renderGraph, lutDesc, "_InternalGradingLut", true);
      
        
        
        //ToDo:待优化
        ToneMappingSettings.Mode mode = settings.ToneMapping.mode;
        passName = Enum.GetName(typeof(PostFXSettings.ToneMappingSettings.Mode), mode );
        cmd.BeginSample(passName);
       
        Pass pass = Pass.ColorGradingNone + (int)mode;
        
        // Debug.Log(passName);
        // Debug.Log((int) pass);
        // var templaId = scoureId;

        // Draw(templaId, scoureId, pass);
        
        // Draw(source, source, Pass.Copy);
        Draw(source, scoureId, pass);
        cmd.ReleaseTemporaryRT(colorGradingLUTId);
    }
    
    //Base funtion for Rendering
    #region Draw
    void Draw (RTHandle from, RTHandle to, Pass pass) {
        cmd.SetGlobalTexture(fxSourceId, from);
        Blitter.BlitCameraTexture(cmd, from, to, settings.Material, (int)pass);
    }
    void Draw (RTHandle from, RTHandle to, int pass) {
        cmd.SetGlobalTexture(fxSourceId, from);
        Blitter.BlitCameraTexture(cmd, from, to,  settings.BlurMaterial, pass);
    }
    #endregion

    public void Dispose()
    {
        bloomFinalId?.Release();
        destinId?.Release();
        foreach (var handle in m_BloomMipDown)
            handle?.Release();
        foreach (var handle in m_BloomMipUp)
            handle?.Release();
    }
}