using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private bool hasSafetyPin = true;
    private bool hasSafetyLever = true;


    public void OnSafetyPinOff()
    {
        hasSafetyPin = false;
    }

    public void OnSafetyPinOn()
    {
        hasSafetyPin = true;
    }

    public void OnSafetyLeverOff()
    {
        if (hasSafetyPin)
            return;

        StartCoroutine(Process());
    }

    private void Fire()
    {
        Debug.Log("¼ö·ùÅº Æø¹ß");
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(5f);
        Fire();
    }
}
