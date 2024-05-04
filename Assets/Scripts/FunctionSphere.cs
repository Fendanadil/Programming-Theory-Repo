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
        compatibleRing = "FunctionRing";
        sphereMR.material.mainTexture = m_sphereTexture;
        sphereMR.material.mainTextureScale = new Vector3(1.5f, 1.0f);
    }

    public Color ReturnColorResult(Color c1, Color c2)
    {
        switch (m_sphereTexture.name)
        {
            case "plus":
                float newRed = c1.r + c2.r;
                float newGreen = c1.g + c2.g;
                float newBLue = c1.b + c2.b;

                return new Color(SanitizeColorValue(c1.r + c2.r), SanitizeColorValue(c1.g + c2.g), SanitizeColorValue(c1.b + c2.b) , 1);
            case "minus":
                return new Color(SanitizeColorValue(c1.r - c2.r), SanitizeColorValue(c1.g - c2.g), SanitizeColorValue(c1.b - c2.b), 1);
            case "multiply":
                return new Color(c1.r * c2.r, c1.g * c2.g, c1.b * c2.b, 1);
            case "avg":
                return new Color((c1.r + c2.r) / 2, (c1.g + c2.g) / 2, (c1.b + c2.b) / 2, 1);
            default:
                Debug.LogError("unknown function sphere!");
                return sphereColor;
        }
        
    }

    private float SanitizeColorValue(float c) 
    {
        if (c < 0)
        {
            return 0;
        }
        else if (c > 1)
        {
            return 1;
        }
        else 
        {
            return c;
        }

    }

}
