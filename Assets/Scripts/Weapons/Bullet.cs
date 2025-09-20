using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(LifeCycle());
    }

    protected void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
