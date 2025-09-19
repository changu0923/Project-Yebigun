using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandDetector : MonoBehaviour
{
    [SerializeField] private InteractionLayerMask desiredFilterMask;
    private InteractionLayerMask getOriginalMask;

    private void OnTriggerEnter(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {            
            interactor.interactionLayers &= ~desiredFilterMask;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        XRDirectInteractor interactor = other.GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            interactor.interactionLayers |= desiredFilterMask;
        }
    }
}
