using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFull : MonoBehaviour
{
    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Eject()
    {
        Vector3 dir = transform.right * 2f + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1.5f), Random.Range(-0.25f, 0.25f));
        rb.AddForce(dir, ForceMode.Impulse);
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
