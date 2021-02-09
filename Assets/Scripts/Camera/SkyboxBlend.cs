using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class SkyboxBlend : MonoBehaviour
{
    public ReflectionProbe m_blendSource;
    Texture2D m_blanedTexture;
    public Material mt;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        if(m_blendSource == null)
        {
            Debug.LogError("no probe.");
        }
        m_blanedTexture = (Texture2D)m_blendSource.bakedTexture;
        m_blanedTexture.requestedMipmapLevel = 2;
        
        mt.SetTexture("_Blend", m_blanedTexture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        //Graphics.Blit(source, destination, mt);
    }
}
