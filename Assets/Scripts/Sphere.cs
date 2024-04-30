using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Sphere : MonoBehaviour
{
    private MeshRenderer sphereMR;
    private Rigidbody sphereRB;
    [SerializeField] private GameObject sphereParticles;
    private ParticleSystem spherePS;

    private Vector3 dragScreenSpace;
    private Vector3 dragOffset;

    private float fadeTime = 0.25f;
    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private Vector3 spawnPosition;
    private bool hasColor = false;
    private Color m_sphereColor;
    public Color sphereColor
    {
        get
        {
            return m_sphereColor;
        }
        set
        {
            if (hasColor)
            {
                Debug.LogError("The color of the sphere has already been set and can't be changed anymore!");
            }
            else
            {
                m_sphereColor = value;
                hasColor = true;
            }
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        // init
        sphereMR = GetComponent<MeshRenderer>();
        sphereRB = GetComponent<Rigidbody>();
        spherePS = sphereParticles.GetComponent<ParticleSystem>();
        var main = spherePS.main;

        // color
        sphereMR.material.color = new Color(m_sphereColor.r, m_sphereColor.g, m_sphereColor.b, 0);

        // set the spawn position
        spawnPosition = gameObject.transform.position;

        // set attributes for the particle spawner
        main.startColor = m_sphereColor;
        main.duration = fadeTime;

        // spawn the sphere
        SpawnSphere(spawnPosition);
    }

    private void Update()
    {
        if (isFadingOut) 
        {
            FadeOutSphere();
        }

        if (isFadingIn)
        {
            FadeInSphere();
        }

    }

    private void FadeOutSphere() 
    {
        Color thisColor = sphereMR.material.color;
        float fadeAmount = thisColor.a - ((1 / fadeTime) * Time.deltaTime);
        thisColor = new Color(thisColor.r, thisColor.g, thisColor.b, fadeAmount);
        sphereMR.material.color = thisColor;
        if (thisColor.a <= 0)
        {
            // stop the fading
            isFadingOut = false;

            // respawn the sphere at it's starting location
            SpawnSphere(spawnPosition);
        }
    }

    private void FadeInSphere() 
    {
        Color thisColor = sphereMR.material.color;
        float fadeAmount = thisColor.a + ((1 / fadeTime) * Time.deltaTime);
        thisColor = new Color(thisColor.r, thisColor.g, thisColor.b, fadeAmount);
        sphereMR.material.color = thisColor;
        if (thisColor.a >= 1) { isFadingIn = false; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            // disolve the sphere and trigger respawn
            DisolveSpehre(true);
        }
    }

    // Disolve the spehre by playing the particle effect and fading the visibility?
    private void DisolveSpehre(bool doRespawn) 
    {
        // stop any motion
        StopSphere();

        // start particles
        spherePS.Play();

        // turn shadows off
        sphereMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        // set fading state for the update methode
        isFadingOut = true;


    }
    // Spawn the sphere by playing the particle effect and fading the object
    private void SpawnSphere(Vector3 pos) 
    {
        // set fading state for the update methode
        isFadingIn = true;

        // position sphere at spawn position
        gameObject.transform.position = pos;

        // turn shadows on
        sphereMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        // start particle effect
        spherePS.Play();
    }

    private void StopSphere() 
    {
        // stop any motion
        sphereRB.velocity = Vector3.zero;
        sphereRB.angularVelocity = Vector3.zero;
    }

    // on click
    private void OnMouseDown()
    {
        StopSphere();
        dragScreenSpace = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        dragOffset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragScreenSpace.z));
    }

    // on drag
    private void OnMouseDrag()
    {
        Vector3 curScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragScreenSpace.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPosition) + dragOffset;
        transform.position = new Vector3(curPosition.x, 3, curPosition.z);
    }

    // function of the sphere
    public virtual void ApplySphereFunction() 
    { 

    }
}
