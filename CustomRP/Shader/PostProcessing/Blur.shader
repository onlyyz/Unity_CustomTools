Shader "URP14/Custom RP/Blur"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
    }

    SubShader
    {
        Cull Off
        ZTest Always
        ZWrite Off

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // The Blit.hlsl file provides the vertex shader (Vert),
        // input structure (Attributes) and output strucutre (Varyings)
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        #include "Assets/CustomRP/Library/Blur.hlsl"
        ENDHLSL

        Pass
        {
            Name "Copy"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment CopyPassFragment
            ENDHLSL
        }
        Pass
        {
            Name "Box Blur"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment BoxPassFragment
            ENDHLSL
        }
        Pass
        {
            Name "Kawase"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment KawasPassFragment
            ENDHLSL
        }
        Pass
        {
            Name "Gaussian Horizontal"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment GaussianHorizontalPassFragment
            ENDHLSL
        }

        Pass
        {
            Name "Gaussian Vertical"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment GaussianVerticalPassFragment
            ENDHLSL
        }
        Pass
        {
            Name "Dual Kawas Down Sample"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment GaussianHorizontalPassFragment
            ENDHLSL
        }

        Pass
        {
            Name "Dual Kawas Up Sample"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment GaussianVerticalPassFragment
            ENDHLSL
        }


    }
}