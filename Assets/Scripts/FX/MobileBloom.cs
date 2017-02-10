using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MobileBloom : MonoBehaviour {

    public float intensity = 0.3f;
    public Color colorMix = Color.white;
    public float colorMixBlend = 0.25f;
    public float agonyTint = 0.0f;

    private Shader bloomShader = null;
    private Material apply = null;
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

	// Use this for initialization
	void Start () {
        FindShaders();
        CheckSupport();
        CreateMaterials();
	}

    internal void FindShaders()
    {
        if(!bloomShader)
        {
            //放到Resources文件夹中确保打到apk中
            bloomShader = Shader.Find("Hidden/MobileBloom");
        }
    }

    internal void CreateMaterials()
    {
        if(!apply)
        {
            apply = new Material(bloomShader);
            apply.hideFlags = HideFlags.DontSave;
        }
    }

    internal bool Supported()
    {
        return SystemInfo.supportsImageEffects && bloomShader.isSupported;
    }

    internal bool CheckSupport()
    {
        if(!Supported())
        {
            enabled = false;
            return false;
        }
        rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565) ? RenderTextureFormat.RGB565 : RenderTextureFormat.Default;
        return true;
    }

    internal void onDisable()
    {
        if(apply)
        {
            DestroyImmediate(apply);
            apply = null;
        }
    }

    internal void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //Blit(src,dest,mat,pass)函数的作用，按照Unity官方API的说法是将src这个RT用mat这个材质中的某个pass渲染，然后复制到dest
#if UNITY_EDITOR
        FindShaders();
        CheckSupport();
        CreateMaterials();
#endif

        agonyTint = Mathf.Clamp01(agonyTint - Time.deltaTime * 2.75f);

        RenderTexture tempRtLowA = RenderTexture.GetTemporary(src.width/4, src.height/4, 0, rtFormat);
        RenderTexture tempRtLowB = RenderTexture.GetTemporary(src.width / 4, src.height / 4, 0, rtFormat);

        apply.SetColor("_ColorMix", colorMix);
        apply.SetVector("_Parameter", new Vector4(colorMixBlend * 0.25f, 0.0f, 0.0f, 1.0f - intensity - agonyTint));

        //Blit 这个函数就像过转化器一样，source图片经过它的处理变成了dest图片
        Graphics.Blit(src, tempRtLowA, apply, agonyTint < 0.5f ? 1 : 5);
        Graphics.Blit(tempRtLowA, tempRtLowB, apply, 2);
        Graphics.Blit(tempRtLowB, tempRtLowA, apply, 3);

        apply.SetTexture("_Bloom", tempRtLowA);
        Graphics.Blit(src, dest, apply, 0);

        RenderTexture.ReleaseTemporary(tempRtLowA);
        RenderTexture.ReleaseTemporary(tempRtLowB);
    }

    internal void OnDamage()
    {
        agonyTint = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
