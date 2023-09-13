using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "URP14/Custom Post FX Settings")]
public class PostFXSettings : ScriptableObject {

	//HideInInspector,
	[Reload("Assets/CustomRP/Shader/PostProcessing/PostFXStack.shader")]
	public Shader shader;
	//HideInInspector,
	[Reload("Assets/CustomRP/Shader/PostProcessing/Blur.shader")]
	public Shader Blurshader;
	
	[Serializable]
	public struct BlurSettings
	{
		public enum PassEnum {
			None,
			Box,
			Kawase,
			Gaussian,
			Dualkawase = Gaussian +2,
			TestBlur,
			Bokeh,
			TiltShift,
			Iris,
			Grainy,
			Radial,
			Directional
		}
		public PassEnum Pass => BlurPass;
		[SerializeField]  PassEnum BlurPass;
		
		[Range(0f, 8f),Tooltip("模糊的迭代次数")]
		public int maxIterations;
		[Range(1f,8f), Tooltip("模糊范围")] 
		public float BlurRange;
		[Min(0f),Tooltip("模糊强度")]
		public float intensity;

		[Tooltip("升降采样")]
		public bool upDownSample;
		[Min(1f),Tooltip("降采样分辨率钳制")]
		public int downscaleLimit;
	}
	
	[SerializeField]  BlurSettings blur;
	public BlurSettings Blur => blur;
	
	[Serializable]
	public struct BloomSettings {
		//跟随Buffer的渲染比例
		public bool ignoreRenderScale;
		[Tooltip("平滑伪影")]
		public bool bicubicUpsampling;

		[Min(0f)]
		public float threshold;

		[Range(0f, 1f)]
		public float thresholdKnee;

		// [Min(0f)]
		// public float intensity;

		public bool fadeFireflies;

		public enum Mode { Additive, Scattering }

		public Mode mode;

		[Range(0.05f, 0.95f)]
		public float scatter;
	}

	[SerializeField]
	BloomSettings bloom = new BloomSettings {
		scatter = 0.7f
	};

	public BloomSettings Bloom => bloom;

	[Serializable,Tooltip("color adjust")]
	public struct ColorAdjustmentsSettings {

		public float postExposure;

		[Range(-100f, 100f)]
		public float contrast;

		[ColorUsage(false, true)]
		public Color colorFilter;

		[Range(-180f, 180f)]
		public float hueShift;

		[Range(-100f, 100f)]
		public float saturation;
	}

	[SerializeField]
	ColorAdjustmentsSettings colorAdjustments = new ColorAdjustmentsSettings {
		colorFilter = Color.white
	};
	public ColorAdjustmentsSettings ColorAdjustments => colorAdjustments;
	
	//白平衡
	[Serializable,Tooltip("白平衡")]
	public struct WhiteBalanceSettings {

		[Range(-100f, 100f)]
		public float temperature, tint;
	}

	[SerializeField]
	WhiteBalanceSettings whiteBalance = default;

	public WhiteBalanceSettings WhiteBalance => whiteBalance;
	//Split Toning
	[Serializable,Tooltip("color adjust")]
	public struct SplitToningSettings {

		[ColorUsage(false)]
		public Color shadows, highlights;

		[Range(-100f, 100f)]
		public float balance;
	}

	[SerializeField]
	SplitToningSettings splitToning = new SplitToningSettings {
		shadows = Color.gray,
		highlights = Color.gray
	};

	public SplitToningSettings SplitToning => splitToning;
//通道混合器
	[Serializable]
	public struct ChannelMixerSettings {

		public Vector3 red, green, blue;
	}

	[SerializeField]
	ChannelMixerSettings channelMixer = new ChannelMixerSettings {
		red = Vector3.right,
		green = Vector3.up,
		blue = Vector3.forward
	};

	public ChannelMixerSettings ChannelMixer => channelMixer;
	// 阴影中调高光
	[Serializable]
	public struct ShadowsMidtonesHighlightsSettings {

		[ColorUsage(false, true)]
		public Color shadows, midtones, highlights;

		[Range(0f, 2f)]
		public float shadowsStart, shadowsEnd, highlightsStart, highLightsEnd;
	}

	[SerializeField]
	ShadowsMidtonesHighlightsSettings
		shadowsMidtonesHighlights = new ShadowsMidtonesHighlightsSettings {
			shadows = Color.white,
			midtones = Color.white,
			highlights = Color.white,
			shadowsEnd = 0.3f,
			highlightsStart = 0.55f,
			highLightsEnd = 1f
		};

	public ShadowsMidtonesHighlightsSettings ShadowsMidtonesHighlights =>
		shadowsMidtonesHighlights;
	
	//Tone Mapping

	[Serializable]
	public struct ToneMappingSettings {

		public enum Mode { None, ACES, Neutral, Reinhard }

		public Mode mode;
	}

	[SerializeField]
	ToneMappingSettings toneMapping = default;

	public ToneMappingSettings ToneMapping => toneMapping;

	[NonSerialized]
	Material material;

	public Material BlurMaterial {
		get {
			if (material == null && Blurshader != null) {
				material = CoreUtils.CreateEngineMaterial(Blurshader);
				material.hideFlags = HideFlags.HideAndDontSave;
			}
			return material;
		}
	}
	
	public Material Material {
		get {
			if (material == null && shader != null) {
				material = CoreUtils.CreateEngineMaterial(shader);
				material.hideFlags = HideFlags.HideAndDontSave;
			}
			
			return material;
		}
	}
}