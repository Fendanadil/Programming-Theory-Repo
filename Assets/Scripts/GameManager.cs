using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject colorSphere;
    private List<Color> sphereColors = new List<Color>();
    [SerializeField] GameObject functionSphere;
    [SerializeField] private List<Texture2D> sphereTextures = new List<Texture2D>(); // haven't found another way to assign textures...
    private float spawnDelay = 1.0f;
    private Vector3 spawnPos1 = new Vector3(-4.3f, 5f, 4.0f);
    private Vector3 spawnPos2 = new Vector3(4.3f, 5f, 4.0f);
    private Color functionSphereColor = new Color(1, 1, 1);

    private void Awake()
    {
        // define the colors to spawn
        sphereColors.Add(new Color(1, 0, 0));
        sphereColors.Add(new Color(0, 1, 0));
        sphereColors.Add(new Color(0, 0, 1));
        sphereColors.Add(new Color(1, 1, 0));
        sphereColors.Add(new Color(1, 0, 1));
        sphereColors.Add(new Color(0, 1, 1));
        sphereColors.Add(new Color(1, 1, 1));
        sphereColors.Add(new Color(0, 0, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        // spawn spheres till all the colors are used up
        StartCoroutine("SpawnColorSphere");

        // spawn spheres till all the textures are used up
        StartCoroutine("SpawnFunctionSphere");

    }

    IEnumerator SpawnColorSphere() 
    {
        while (sphereColors.Count > 0) {
            yield return new WaitForSeconds(spawnDelay);
            SpawnSphere(sphereColors[0], spawnPos1);
            sphereColors.RemoveAt(0);
        }
    }

    IEnumerator SpawnFunctionSphere()
    {
        while (sphereTextures.Count > 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnSphere(sphereTextures[0], spawnPos2);
            sphereTextures.RemoveAt(0);
        }
    }

    private void SpawnSphere(Color c, Vector3 p) 
    {
        // spawn the new spwhere in an inactive state to prevent Awake from runngin
        colorSphere.SetActive(false);
        var mySphere = Instantiate(colorSphere, p, colorSphere.transform.rotation);
        colorSphere.SetActive(true);

        // assign the color of the sphere
        mySphere.GetComponent<ColorSphere>().sphereColor = sphereColors[0];

        // activate sphere to trigger Awake
        mySphere.SetActive(true);
    }

    // OVERLOAD for material 
    private void SpawnSphere(Texture2D t, Vector3 p)
    {
        // spawn the new spwhere in an inactive state to prevent Awake from runngin
        functionSphere.SetActive(false);
        var mySphere = Instantiate(functionSphere, p, functionSphere.transform.rotation);
        functionSphere.SetActive(true);

        // assign texture
        mySphere.GetComponent<FunctionSphere>().sphereColor = functionSphereColor;
        mySphere.GetComponent<FunctionSphere>().sphereTexture = t;

        // activate sphere to trigger Awake
        mySphere.SetActive(true);

    }
}
