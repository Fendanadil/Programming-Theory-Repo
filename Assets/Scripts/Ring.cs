using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private float pullRadius = 2f;
    private float pullForce = 10.0f;

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

}
