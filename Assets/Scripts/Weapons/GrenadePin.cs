using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePin : MonoBehaviour
{
    [SerializeField] private Transform handTransform;

    public void HandPresentOn()
    {
        handTransform.gameObject.SetActive(true);
    }

    public void HandPresentOff()
    {
        handTransform.gameObject.SetActive(false);
    }
}
