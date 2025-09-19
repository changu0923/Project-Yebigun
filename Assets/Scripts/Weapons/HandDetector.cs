using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandDetector : MonoBehaviour
{
    private bool isDetected = false;
    [SerializeField] private InteractionLayerMask desiredFilterMask;
    private InteractionLayerMask getOriginalMask;

    public bool IsDetected { get => isDetected; }

    private void OnTriggerEnter(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {            
            isDetected = true;
            interactor.interactionLayers &= ~desiredFilterMask;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            isDetected = false;
            interactor.interactionLayers |= desiredFilterMask;
        }
    }
}
