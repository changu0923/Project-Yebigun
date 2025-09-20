using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject vfxPrefab;

    public void SpawnVFX()
    {
        GameObject obj = Instantiate(vfxPrefab, startPos.position, Quaternion.identity);
        StartCoroutine(LifeCycle(obj));
    }

    IEnumerator LifeCycle(GameObject obj)
    {
        yield return new WaitForSeconds(.3f);
        Destroy(obj);
    }
}
