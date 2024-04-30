using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject sphere;
    private List<Color> sphereColors = new List<Color>();
    private float spawnDelay = 1.0f;
    private Vector3 spawnPos1 = new Vector3(-4.3f, 4.5f, 4.0f);
    private Vector3 spawnPos2 = new Vector3(4.3f, 4.5f, 4.0f);

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
    }

    IEnumerator SpawnColorSphere() 
    {
        while (sphereColors.Count > 0) {
            yield return new WaitForSeconds(spawnDelay);
            SpawnSphere(sphereColors[0], spawnPos1);
            sphereColors.RemoveAt(0);
        }
    }

    private void SpawnSphere(Color c, Vector3 p) 
    {
        // spawn the new spwhere in an inactive state to prevent Awake from runngin
        sphere.SetActive(false);
        var mySphere = Instantiate(sphere, p, sphere.transform.rotation);
        sphere.SetActive(true);

        // assign the color of the sphere
        mySphere.GetComponent<ColorSphere>().sphereColor = sphereColors[0];

        // activate sphere to trigger Awake
        mySphere.SetActive(true);
    }

    // OVERLOAD for material 
    private void SpawnSphere(Material m, Vector3 p)
    {
        // spawn the new spwhere in an inactive state to prevent Awake from runngin
        sphere.SetActive(false);
        var mySphere = Instantiate(sphere);
        sphere.SetActive(true);

        // change the starting conditions for the sphere

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
