using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class Sphere : MonoBehaviour
{
    protected MeshRenderer sphereMR;
    private Rigidbody sphereRB;
    private SphereCollider sphereC;
    [SerializeField] private GameObject sphereParticles;
    private ParticleSystem spherePS;
    [SerializeField] private GameObject sphereLight;
    private Light sphereL;
    [SerializeField] private GameObject sphereLine;
    private LineRenderer sphereLR;

    private float outOfBoundY = -10.0f;

    private float planeY = 3.0f;
    private Plane plane;
    private Ray ray;

    private float lineDrawTime = 0.25f;
    private float lineLength = 1.5f;
    private float sphereRadius;
    private Vector3 realDown;

    private float fadeTime = 0.25f;
    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private bool isPullingLine = false;
    private bool isRetractingLine = false;
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
        sphereC = GetComponent<SphereCollider>();
        sphereRB = GetComponent<Rigidbody>();
        sphereL = sphereLight.GetComponent<Light>();
        sphereLR = sphereLine.GetComponent<LineRenderer>();
        spherePS = sphereParticles.GetComponent<ParticleSystem>();
        var main = spherePS.main;

        // store dimension
        sphereRadius = sphereC.radius;

        // set the spawn position
        spawnPosition = gameObject.transform.position;

        // set material properties
        sphereMR.material.color = new Color(m_sphereColor.r, m_sphereColor.g, m_sphereColor.b, 0);

        // set additional properties
        SetAdditionalProperties(); 
        
        // light color
        sphereL.color = sphereColor;

        // line color
        sphereLR.startColor = m_sphereColor;

        // set attributes for the particle spawner
        main.startColor = m_sphereColor;
        main.duration = fadeTime;

        // spawn the sphere
        SpawnSphere(spawnPosition);
    }

    public virtual void SetAdditionalProperties() 
    { 
    
    }
        

    private void Update()
    {   
        // fading of the sphere
        if (isFadingOut) 
        {
            FadeOutSphere();
        }

        if (isFadingIn)
        {
            FadeInSphere();
        }

        // line drawing
        if (isPullingLine) 
        {
            PullLine();
        }

        if (isRetractingLine)
        {
            RetractLine();
        }

        // recover spheres that got out of bound
        if (gameObject.transform.position.y < outOfBoundY) 
        {
            DisolveSphere(true);
        }

    }

    private void FadeOutSphere() 
    {
        Color thisColor = sphereMR.material.color;
        float alphaValue = thisColor.a - (1 / fadeTime * Time.deltaTime);
        thisColor = new Color(thisColor.r, thisColor.g, thisColor.b, alphaValue);
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
        float alphaValue = thisColor.a + (1 / fadeTime * Time.deltaTime);
        if (alphaValue > 1) {
            alphaValue = 1;
            isFadingIn = false;
        }
        thisColor = new Color(thisColor.r, thisColor.g, thisColor.b, alphaValue);
        sphereMR.material.color = thisColor;
        
    }


    private void SetLineStart()
    {
        realDown = gameObject.transform.InverseTransformVector(Vector3.down);
        Vector3 startingPoint = realDown * sphereRadius;
        
        sphereLR.SetPosition(0, startingPoint);
        sphereLR.SetPosition(1, startingPoint);
    }

    private void PullLine() 
    {
        float thisLength = Vector3.Distance(sphereLR.GetPosition(0), sphereLR.GetPosition(1)) + (1 / lineDrawTime * Time.deltaTime);
        if (thisLength > lineLength) 
        {
            thisLength = lineLength;
            isPullingLine = false;
        }
        Vector3 endPoint = sphereLR.GetPosition(0) + (realDown * thisLength);
        sphereLR.SetPosition(1, endPoint);
    }

    private void RetractLine()
    {
        float thisLength = Vector3.Distance(sphereLR.GetPosition(0), sphereLR.GetPosition(1)) - (1 / lineDrawTime * Time.deltaTime);
        if (thisLength < 0)
        {
            thisLength = 0;
            isRetractingLine = false;
            sphereLine.SetActive(false);
        }
        Vector3 endPoint = sphereLR.GetPosition(0) + (realDown * thisLength);
        sphereLR.SetPosition(1, endPoint);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // disolve the sphere and trigger respawn
            DisolveSphere(true);
        }
         
    }

    // Disolve the spehre by playing the particle effect and fading the visibility?
    private void DisolveSphere(bool doRespawn) 
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

    // when picking up
    private void OnMouseDown()
    {
        // stop current movement/rotation and define drag & drop plane
        StopSphere();
        sphereRB.freezeRotation = true;
        plane = new Plane(Vector3.up, Vector3.up * planeY);

        // stop gravity
        sphereRB.useGravity = false;
        
        // set starting conditions for the line drawing
        sphereLine.SetActive(true);
        isPullingLine = true;

        SetLineStart();
    }

    // on drag
    private void OnMouseDrag()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance)) 
        {
            sphereRB.velocity = (ray.GetPoint(distance) - gameObject.transform.position) * 10; 
        }
    }

    // whenletting go
    private void OnMouseUp()
    {
        //StopSphere();
        sphereRB.freezeRotation = false;
        isPullingLine = false;
        isRetractingLine = true;

        // restore gravity
        sphereRB.useGravity = true;
    }

    // function of the sphere
    public abstract void ApplySphereFunction();
}
