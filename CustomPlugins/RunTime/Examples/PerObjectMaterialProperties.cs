using Sirenix.OdinInspector;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
	private static int
		baseColorId = Shader.PropertyToID("_BaseColor"),
		cutoffId = Shader.PropertyToID("_Cutoff"),
		metallicId = Shader.PropertyToID("_Metallic"),
		smoothnessId = Shader.PropertyToID("_Smoothness"),
		emissionColorId = Shader.PropertyToID("_EmissionColor"),
		BaseMapId = Shader.PropertyToID("_BaseMap");

	static MaterialPropertyBlock block;

	[SerializeField,LabelText("修改贴图")] 
    Texture2D baseMap;
    [SerializeField,LabelText("修改颜色")] 
	Color baseColor = Color.white;

	[SerializeField, Range(0f, 1f)]
	float alphaCutoff = 0.5f, metallic = 0f, smoothness = 0.5f;

	[SerializeField, ColorUsage(false, true)]
	Color emissionColor = Color.black;

	void Awake () {
		OnValidate();
	}

	void OnValidate () {
		if (block == null) {
			block = new MaterialPropertyBlock();
		}
		block.SetColor(baseColorId, baseColor);
		block.SetFloat(cutoffId, alphaCutoff);
		block.SetFloat(metallicId, metallic);
		block.SetFloat(smoothnessId, smoothness);
		block.SetColor(emissionColorId, emissionColor);
		if(baseMap !=null) 
			block.SetTexture(BaseMapId, baseMap);
		
		GetComponent<Renderer>().SetPropertyBlock(block);
	}
}