#ifndef CUSTOM_BLUR_INCLUDE
#define CUSTOM_BLUR_INCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

TEXTURE2D_X(_CameraOpaqueTexture);
SAMPLER(sampler_CameraOpaqueTexture);


TEXTURE2D_X(_PostFXSource);
TEXTURE2D_X(_PostFXSource2);
SAMPLER(sampler_PostFXSource);
SAMPLER(sampler_PostFXSource2);

float4 _PostFXSource_TexelSize;
float _BlurRange;

SAMPLER(sampler_linear_clamp);
SAMPLER(sampler_point_clamp);

float4 GetSourceTexelSize()
{
    return _PostFXSource_TexelSize;
}

float4 GetSourceBicubic(float2 screenUV)
{
    return SampleTexture2DBicubic(
        TEXTURE2D_ARGS(_PostFXSource, sampler_linear_clamp), screenUV,
        _PostFXSource_TexelSize.zwxy, 1.0, 0.0
    );
}

//TODO:_ProjectionParams.x uv 翻转？
//Bloom
float4 GetSource(float2 screenUV)
{
    return SAMPLE_TEXTURE2D_LOD(_PostFXSource, sampler_PostFXSource, screenUV, 0);
}

float4 GetSource2(float2 screenUV)
{
    return SAMPLE_TEXTURE2D_LOD(_PostFXSource2, sampler_PostFXSource2, screenUV, 0);
}


float4 GaussianHorizontalPassFragment(Varyings input) : SV_TARGET
{
    float3 color = 0.0;
    float offsets[] = {
        -4.0, -3.0, -2.0, -1.0, 0.0, 1.0, 2.0, 3.0, 4.0
    };
    float weights[] = {
        0.01621622, 0.05405405, 0.12162162, 0.19459459, 0.22702703,
        0.19459459, 0.12162162, 0.05405405, 0.01621622
    };
    for (int i = 0; i < 9; i++)
    {
        float offset = offsets[i] * 2.0 * GetSourceTexelSize().x;
        color += GetSource(input.texcoord + _BlurRange * float2(offset, 0.0)).rgb * weights[i];
    }
    return float4(color, 1.0);
}

float4 GaussianVerticalPassFragment(Varyings input) : SV_TARGET
{
    float3 color = 0.0;
    float offsets[] = {
        -3.23076923, -1.38461538, 0.0, 1.38461538, 3.23076923
    };
    float weights[] = {
        0.07027027, 0.31621622, 0.22702703, 0.31621622, 0.07027027
    };
    for (int i = 0; i < 5; i++)
    {
        float offset = offsets[i] * GetSourceTexelSize().y;
        color += GetSource(input.texcoord + _BlurRange * float2(0.0, offset)).rgb * weights[i];
    }
    return float4(color, 1.0);
}

float4 BoxPassFragment(Varyings input) : SV_TARGET
{
    float4 d = GetSourceTexelSize().xyxy * float4(1.0, 1.0, -1.0, 0.0);

    float4 color = 0.0;
    color = GetSource(input.texcoord - d.xy * _BlurRange);
    color += GetSource(input.texcoord - d.wy * _BlurRange) * 2.0; // 1 MAD
    color += GetSource(input.texcoord - d.zy * _BlurRange); // 1 MAD

    color += GetSource(input.texcoord + d.zw * _BlurRange) * 2.0; // 1 MAD
    color += GetSource(input.texcoord) * 4.0; // 1 MAD
    color += GetSource(input.texcoord + d.xw * _BlurRange) * 2.0; // 1 MAD

    color += GetSource(input.texcoord + d.zy * _BlurRange);
    color += GetSource(input.texcoord + d.wy * _BlurRange) * 2.0; // 1 MAD
    color += GetSource(input.texcoord + d.xy * _BlurRange);

    // return float4(1.0,0.0,1.0,1.0);
    return color * 1 / 16;
}

//KawasPass
float4 KawasPassFragment(Varyings input) : SV_TARGET
{
    float4 color = GetSource(input.texcoord);
    color += GetSource(input.texcoord + float2(-1, -1) * GetSourceTexelSize().xy * _BlurRange);
    color += GetSource(input.texcoord + float2(1, -1) * GetSourceTexelSize().xy * _BlurRange);
    color += GetSource(input.texcoord + float2(-1, 1) * GetSourceTexelSize().xy * _BlurRange);
    color += GetSource(input.texcoord + float2(1, 1) * GetSourceTexelSize().xy * _BlurRange);
    color /= 5;
    return color;
}

//DualKawas Pass
half4 DualKawasDownSample(Varyings input): SV_TARGET
{
    float2 uvsize = GetSourceTexelSize() * 0.5;
    half4 sum = GetSource(input.texcoord) * 4;
    ////top right
    sum += GetSource(input.texcoord - uvsize * float2(1 + _BlurRange, 1 + _BlurRange));
    //bottom left
    sum += GetSource(input.texcoord + uvsize * float2(1 + _BlurRange, 1 + _BlurRange));
    //top left
    sum += GetSource(input.texcoord - float2(uvsize.x, -uvsize.y) * float2(1 + _BlurRange, 1 + _BlurRange));
    //bottom right
    sum += GetSource(input.texcoord - float2(uvsize.x, -uvsize.y) * float2(1 + _BlurRange, 1 + _BlurRange));

    return sum * 0.125;
}

half4 DualKawasUpSample(Varyings input): SV_TARGET
{
    float2 _MainTex_TexelSize = GetSourceTexelSize() * 0.5;
    half4 sum = 0;
    sum += GetSource(input.texcoord + float2(-_MainTex_TexelSize.x * 2, 0) * _BlurRange);
    sum += GetSource(input.texcoord + float2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y) * _BlurRange) * 2;
    sum += GetSource(input.texcoord + float2(0, _MainTex_TexelSize.y * 2) * _BlurRange);
    sum += GetSource(input.texcoord + _MainTex_TexelSize * _BlurRange) * 2;
    sum += GetSource(input.texcoord + float2(_MainTex_TexelSize.x * 2, 0) * _BlurRange);
    sum += GetSource(input.texcoord + float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _BlurRange) * 2;
    sum += GetSource(input.texcoord + float2(0, -_MainTex_TexelSize.y * 2) * _BlurRange);
    sum += GetSource(input.texcoord - _MainTex_TexelSize * _BlurRange) * 2;
    return sum * 0.0833;
}

//Bokeh Blur
half4 _GoldenRot;
half4 _Params;

#define _Iteration _Params.x
#define _Radius _Params.y
#define _PixelSize _Params.zw

half4 BokehBlur(Varyings i)
{
    half2x2 rot = half2x2(_GoldenRot);
    half4 accumulator = 0.0;
    half4 divisor = 0.0;

    half r = 1.0;
    half2 angle = half2(0.0, _Radius);

    for (int j = 0; j < _Iteration; j++)
    {
        r += 1.0 / r;
        angle = mul(rot, angle);
        half4 bokeh = GetSource(float2(i.texcoord + _PixelSize * (r - 1.0) * angle));
        accumulator += bokeh * bokeh;
        divisor += bokeh;
    }
    return accumulator / divisor;
}

half4 BokehFragment(Varyings input): SV_Target
{
    return BokehBlur(input);
}


//Copy Pass
half4 CopyPassFragment(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    return GetSource(input.texcoord);
    // return float4( input.texcoord,0.0,1.0);
}

#endif
