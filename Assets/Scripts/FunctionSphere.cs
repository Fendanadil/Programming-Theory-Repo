using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionSphere : Sphere
{
    private bool hasTexture = false;
    private Texture2D m_sphereTexture;
    public Texture2D sphereTexture
    {
        get
        {
            return m_sphereTexture;
        }
        set
        {
            if (hasTexture)
            {
                Debug.LogError("The texture of the sphere has already been set and can't be changed anymore!");
            }
            else
            {
                m_sphereTexture = value;
                hasTexture = true;
            }
        }
    }
    public override void SetAdditionalProperties()
    {
        sphereMR.material.mainTexture = m_sphereTexture;
        sphereMR.material.mainTextureScale = new Vector3(1.5f, 1.0f);
    }

    public override void ApplySphereFunction()
    {

    }

}
