using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScreenBloom : MonoBehaviour
{
    //Color
    public Color m_Color;

    public Shader shader;
    private Material m_Material;
    private bool m_IsActive = false;
    private float m_Offset = 0.0f;
    private long m_TotalTime = 0;

    //Properties
    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;//HideFlags.HideAndDontSave 对象不是有也不会被Resources.UnloadUnusedAssets卸载
            }
            return m_Material;
        }
    }

    //Methods
    private void Update()
    {
        if (m_IsActive)
        {
            //time.deltatime的意思是，每一帧的时间，在update中每一次减一个deltatime，update本身就是每一帧运行一次的
            float offset = m_Offset * Time.deltaTime * 1000;
            m_Color += new Color(offset, offset, offset, 0);
            m_TotalTime = (long)(m_TotalTime - Time.deltaTime * 1000);
            if (m_TotalTime < 0)
            {
                m_IsActive = false;
            }
        }
    }

    //OnDisable和OnEnable这两个方法是在gameObject被设置为active true和 false的时候调用的。
    protected void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }

    void OnEnable()
    {
        shader = Shader.Find("Hidden/DFM/BloomSimple");
    }

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetColor("_Color", m_Color);
        //Graphics.Blit：sourceTexture会成为material的_MainTex。
        //这个函数负责从Unity渲染器中抓取当前的render texture，然后使用Graphics.Blit()函数再传递给Shader（通过sourceTexture参数），然后再返回一个处理后的图像再次传递回给Unity渲染器（通过destTexture参数）
        Graphics.Blit(source, destination, material);
    }

    // message
    void DimScreen(long time)
    {
        m_IsActive = true;
        m_TotalTime = time;
        m_Offset = 1.0f / time;
        m_Offset *= -1;
        m_Color = new Color(1, 1, 1, 1);
    }

    void LightScreen(long time)
    {
        m_IsActive = true;
        m_TotalTime = time;
        m_Offset = 1.0f / time;
        m_Color = new Color(0, 0, 0, 1);
    }
}