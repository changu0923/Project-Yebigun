using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresent : MonoBehaviour
{
    [SerializeField] private Transform handPresent;

    public void TurnOn()
    {
        handPresent.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        handPresent.gameObject.SetActive(false);
    }
}