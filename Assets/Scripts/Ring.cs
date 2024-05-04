using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] protected int ringIndex;
    [SerializeField] private GameObject ringLine;
    private LineRenderer ringLR;
    [SerializeField] GameObject targetRing;
    protected GameObject connectedSphere;
    private float pullRadius = 1f;
    private float pullForce = 150.0f;
    private bool isAnimatingLine = false;
    private float lineLength;
    protected float targetLineLength;
    private Vector3 lineDirection;
    private float ringRadius = 0.5f;
    private float drawTime = 1.0f;
    private Color ringColor;
    protected Dictionary<int, Color> connectedColors = new Dictionary<int, Color>();

    private void Awake()
    {
        // init
        ringLR = ringLine.GetComponent<LineRenderer>();

        // calculate line variables
        lineDirection = (targetRing.transform.position - gameObject.transform.position).normalized;
        lineLength = Vector3.Distance(targetRing.transform.position, gameObject.transform.position) - (2 * ringRadius);
        Vector3 startPoint = lineDirection * ringRadius;

        // set starting conditions for the line renderer
        ringLR.SetPosition(0, startPoint);
        ringLR.SetPosition(1, startPoint);
    }

    private void Start()
    {

    }

    public void FixedUpdate()
    {
        foreach (Collider collider in Physics.OverlapSphere(gameObject.transform.position, pullRadius)) 
        {
            // only act on spheres
            if (collider.CompareTag("Sphere")) 
            {
                Vector3 forceDirection = gameObject.transform.position - collider.transform.position;
                collider.attachedRigidbody.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }

    public void Update()
    {
        if (isAnimatingLine)
        {
            AnimateLine();
        }
    }

    public void AttachColor(int index, Color c) 
    {
        // check if  the color has already been connected
        if (connectedColors.ContainsKey(index)) {
            Debug.LogError("The color with the index [" + index + "] has already been connected to ring [" + ringIndex + "]");
            return;
        }

        // connect the color
        connectedColors.Add(index, c);

        // check if we have to perform any actions
        UpdateRingStatus();
    }

    public void DetacheColor(int index)
    {
        // onle remove the color if it was attached in the first place
        if (!connectedColors.ContainsKey(index))
        {
            return;
        }

        // detache the color
        connectedColors.Remove(index);

        // retract the connecting line
        RetractLine();
        targetRing.GetComponent<Ring>().UpdateRingStatus();
    }


    virtual public void UpdateRingStatus() 
    {
        InitiateLineDraw();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if a valid sphere is connecting
        if (collision.gameObject.GetComponent<Sphere>().compatibleRing == gameObject.tag)
        {
            connectedSphere = collision.gameObject;
            SetRingColor(connectedSphere.GetComponent<Sphere>().ReturnColorResult());
            UpdateRingStatus();
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        connectedSphere = null;
        RetractLine();
    }

    protected void SetRingColor(Color c) 
    {
        ringColor = c;
        ringLR.material.color = ringColor;
    }

    protected void InitiateLineDraw()
    {
        targetLineLength = lineLength;
        isAnimatingLine = true;
        ringLine.SetActive(true);
    }

    protected void RetractLine()
    {
        targetLineLength = 0;
        isAnimatingLine = true;
        targetRing.GetComponent<Ring>().DetacheColor(ringIndex);
    }

    private void AnimateLine() 
    {
        float currentLength = Vector3.Distance(ringLR.GetPosition(0), ringLR.GetPosition(1));
        int animDirection = (targetLineLength - currentLength) > 0 ? 1 : -1;
        float deltaLength = Time.deltaTime * (1 / drawTime) * animDirection;
        float newLength = currentLength + deltaLength;
        
        // check if we're overshooting the target length
        if (deltaLength * (newLength - targetLineLength) > 0) 
        {
            newLength = targetLineLength;
            isAnimatingLine = false;

            // set the line to inactive when it reaches 0
            if (newLength == 0)
            {
                ringLine.SetActive(false);
            }
            else 
            {
                // connect color
                targetRing.GetComponent<Ring>().AttachColor(ringIndex, ringColor);
            }
        }

        // set new end point
        ringLR.SetPosition(1, ringLR.GetPosition(0) + lineDirection * newLength);

        // apply new texture tiling
        ringLR.material.mainTextureScale = new Vector2(newLength / ringLR.startWidth, 1);
    }

}
