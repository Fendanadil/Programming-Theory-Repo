using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject torchLight;
    private Light torchL;
    [SerializeField] List<Color> colorList;
    private float minDuration = 0.05f;
    private float maxDuration = 0.4f;
    private float offsetRange = 0.1f;
    private float intensityMin = 0.4f;
    private float intensityMax = 0.6f;
    private Vector3 lightAnchorPos;
    private Vector3 targetPos;
    private Color targetCol;
    private float targetIntensity;
    private float transitionSpeed;

    private void Awake()
    {
        // init
        torchL = torchLight.GetComponent<Light>();
        lightAnchorPos = torchLight.transform.position;
        
        // initiate random atttribute
        StartCoroutine("UpdateLightTarget");

    }

    private void Update()
    {
        // fade color
        float r = torchL.color.r + (targetCol.r - torchL.color.r) * Time.deltaTime * transitionSpeed;
        float g = torchL.color.g + (targetCol.g - torchL.color.g) * Time.deltaTime * transitionSpeed;
        float b = torchL.color.b + (targetCol.b - torchL.color.b) * Time.deltaTime * transitionSpeed;
        torchL.color = new Color(r, g, b);

        // fade intensity
        torchL.intensity = torchL.intensity + (targetIntensity - torchL.intensity) * Time.deltaTime * transitionSpeed;

        // tween position
        torchLight.transform.position = torchLight.transform.position + (targetPos - torchLight.transform.position) * Time.deltaTime * transitionSpeed;
    }

    // define new random target values for the light
    IEnumerator UpdateLightTarget()
    { 
        
        while (true)
        {
            float changeDuration = SetRandomTargetValues();
            yield return new WaitForSeconds(changeDuration);
        }
    
    }

    private float SetRandomTargetValues()
    {
        float changeDuration = Random.Range(minDuration, maxDuration);
        transitionSpeed = 1 / changeDuration;

        // color
        targetCol = colorList[Random.Range(0, colorList.Count)];

        // position
        targetPos = lightAnchorPos + new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange));

        // intensity
        targetIntensity = Random.Range(intensityMin, intensityMax);

        return changeDuration;
    }

}
