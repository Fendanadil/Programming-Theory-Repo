using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class FunctionRing : Ring
{
    // POLYMORPHISM
    override public void UpdateRingStatus()
    {
        if (connectedSphere != null && connectedColors.Count == 2)
        {
            SetRingColor(connectedSphere.gameObject.GetComponent<FunctionSphere>().ReturnColorResult(connectedColors[1], connectedColors[2]));
            InitiateLineDraw();
        }
        else 
        {
            if (targetLineLength > 0) 
            {
                RetractLine();
            }
        
        }
    }
}
