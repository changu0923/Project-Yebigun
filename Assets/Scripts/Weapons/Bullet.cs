using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactPrefab;
    protected Rigidbody rb;
    private LayerMask targetLayer;


    protected void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        targetLayer = LayerMask.NameToLayer("Target");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Fire()
    {
        Vector3 dir = transform.forward;
        rb.AddForce(dir * 960f, ForceMode.Impulse);
        StartCoroutine(LifeCycle());
    }

    private void AfterProcess()
    {
        ObjectPoolManager.Instance.Destroy("Bullet", this.gameObject);
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
            var impactPoint = collision.contacts[0];

            Vector3 impactPos = impactPoint.point;
            Vector3 normal = impactPoint.normal;

            GameObject bulletHole = ObjectPoolManager.Instance.Instantiate("BulletHole", bulletImpactPrefab);
            bulletHole.transform.position = impactPos + (normal * 0.01f);
            bulletHole.transform.rotation = Quaternion.LookRotation(-normal);
            collision.gameObject.GetComponent<Target>().OnImpact(bulletHole);
            AfterProcess();
        }
    }
    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        AfterProcess();
    }
}
