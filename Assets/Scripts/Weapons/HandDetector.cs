using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandDetector : MonoBehaviour
{
    public Action<XRDirectInteractor, bool> OnHandDetected;

    private void OnTriggerEnter(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            OnHandDetected?.Invoke(interactor, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            OnHandDetected?.Invoke(interactor, false);
        }
    }
}
