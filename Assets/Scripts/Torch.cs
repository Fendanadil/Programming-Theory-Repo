using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject torchLight;
    private Light torchL;
    [SerializeField] List<Color> colorList;
    public float minDuration = 0.05f;
    public float maxDuration = 0.4f;
    public float offsetRange = 0.2f;
    public float intensityMin = 0.4f;
    public float intensityMax = 0.6f;
    public Vector3 lightAnchorPos;
    public Vector3 targetPos;
    public Color targetCol;
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
