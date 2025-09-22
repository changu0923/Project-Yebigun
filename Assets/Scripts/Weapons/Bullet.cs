using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rb;
    private LayerMask targetLayer;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetLayer = LayerMask.NameToLayer("Target");
        StartCoroutine(LifeCycle());
    }

    public void Fire()
    {
        Vector3 dir = transform.forward;
        rb.AddForce(dir * 960f, ForceMode.Impulse);
    }
    
    protected void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == targetLayer)
        {
            // Debug.Log($"{collision.gameObject.transform.name}");
        }
    }
    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
