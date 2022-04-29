using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MotionBlur : PostEffectsBase
{
    public Shader motionBlurShader;
    private Material motionBlurMaterial = null;

    public Material material
    {
        get
        {
            motionBlurMaterial = CheckShaderAndCreateMaterial(motionBlurShader, motionBlurMaterial);
            return motionBlurMaterial;
        }
    }

    /// <summary>
    /// 值越大，运动拖尾的效果越明显，为了防止拖尾效果完全替代当前帧渲染结果，把他的值截取在0.0~0.9
    /// </summary>
    [Range(0.0f, 0.9f)] public float blurAmount = 0.5f;

    /// <summary>
    /// 保存当前渲染叠加结果
    /// </summary>
    private RenderTexture accumulationTexture;

    void OnDisable()
    {
        DestroyImmediate(accumulationTexture);
    }

    /// <summary>
    /// 抓取屏幕图像
    /// </summary>
    /// <param name="src">Unity会把当前渲染的得到的图像存储在第一个参数对应的源渲染纹理中</param>
    /// <param name="dest">目标渲染纹理</param>
    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            if (accumulationTexture == null || accumulationTexture.width != src.width ||
                accumulationTexture.height != src.height)
            {
               
                
                DestroyImmediate(accumulationTexture);
                accumulationTexture = new RenderTexture(src.width, src.height, 0);
                accumulationTexture.hideFlags = HideFlags.HideAndDontSave;
                //使用当前帧渲染结果初始化accumulationTexture
                Graphics.Blit(src, accumulationTexture);
            }

            //表明我们需要进行一个渲染纹理的恢复操作
            //使用这个函数明确告诉Unity我知道我在做什么，不用担心
            //恢复操作发生在渲染到纹理而该纹理又没有被提前清空或销毁的情况下
            //作用是保存上一帧渲染结果
            accumulationTexture.MarkRestoreExpected();

            material.SetFloat("_BlurAmount", 1.0f - blurAmount);
            //把当前屏幕图像src叠加到accumulationTexture中
            Graphics.Blit(src, accumulationTexture, material);
            //把结果显示到屏幕上
            Graphics.Blit(accumulationTexture, dest);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}