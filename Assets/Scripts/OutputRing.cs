using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputRing : Ring
{

    [SerializeField] GameObject outputParticles;
    private ParticleSystem outputPS;

    private void Start()
    {
        outputPS = outputParticles.GetComponent<ParticleSystem>();
    }


    override public void UpdateRingStatus()
    {
        if (connectedColors.Count == 1)
        {
            var main = outputPS.main;
            main.startColor = connectedColors[3];
            gameObject.GetComponent<MeshRenderer>().material.color = connectedColors[3] * new Color (1,1,1,0);
            outputPS.Play();
        }
        else 
        {
            outputPS.Stop();
            gameObject.GetComponent<MeshRenderer>().material.color = new Color (1,1,1,0);
        }
    }
}
