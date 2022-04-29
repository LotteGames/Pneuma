using UnityEngine;


[ExecuteInEditMode]//这个特性意为在编辑器模式下也可以运行
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();

        if (isSupported == false)
        {
            NotSupported();
        }
    }

    /// <summary>
    /// 判断是否支持图片特效和渲染纹理
    /// </summary>
    /// <returns></returns>
    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false)
        {
            Debug.LogWarning("This platform does not support image effects or render textures.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 如果检查没有通过，就不执行了
    /// </summary>
    protected void NotSupported()
    {
        enabled = false;
    }

    protected void Start()
    {
        CheckResources();
    }

    /// <summary>
    /// 检查shader并创建材质
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
        {
            return null;
        }

        if (shader.isSupported && material && material.shader == shader)
            return material;

        if (!shader.isSupported)
        {
            return null;
        }

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        return material ? material : null;
    }
}