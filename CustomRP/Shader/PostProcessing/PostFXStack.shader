Shader "URP14/Custom RP/PostFXStack"
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
        #include "Assets/CustomRP/Library/PostFXStack.hlsl"
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
            Name "Bloom Combine"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment BloomCombinePassFragment
            ENDHLSL
        }
        Pass {
			Name "Bloom Prefilter"
			
			HLSLPROGRAM
				#pragma target 3.5
				#pragma vertex Vert
				#pragma fragment BloomPrefilterPassFragment
			ENDHLSL
		}
        
        Pass {
			Name "Color Grading None"
			
			HLSLPROGRAM
				#pragma target 3.5
				#pragma vertex Vert
				#pragma fragment ToneMappingNonePassFragment
			ENDHLSL
		}

		Pass {
			Name "Color Grading ACES"
			
			HLSLPROGRAM
				#pragma target 3.5
				#pragma vertex Vert
				#pragma fragment ToneMappingACESPassFragment
			ENDHLSL
		}

		Pass {
			Name "Color Grading Neutral"
			
			HLSLPROGRAM
				#pragma target 3.5
				#pragma vertex Vert
				#pragma fragment ToneMappingNeutralPassFragment
			ENDHLSL
		}
		
		Pass {
			Name "Color Grading Reinhard"
			
			HLSLPROGRAM
				#pragma target 3.5
				#pragma vertex Vert
				#pragma fragment ToneMappingReinhardPassFragment
			ENDHLSL
		}
    }
}