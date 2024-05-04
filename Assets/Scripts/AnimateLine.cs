using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLine : MonoBehaviour
{
    private LineRenderer thisLR;
    private float scrollSpeedX = 2;
    private float scrollDirectionX = -1;

    // Start is called before the first frame update
    void Start()
    {
        thisLR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float textureOffset = Mathf.Abs(thisLR.material.mainTextureOffset.x) + (scrollSpeedX * Time.deltaTime);
        if (textureOffset > 1) 
        {
            textureOffset = textureOffset - Mathf.Floor(textureOffset);
        }

        thisLR.material.mainTextureOffset = new Vector2(textureOffset * scrollDirectionX, 0);
    }
}
