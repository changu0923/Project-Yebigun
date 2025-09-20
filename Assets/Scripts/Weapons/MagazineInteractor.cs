using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineInteractor : XRGrabInteractable
{
    [SerializeField] Collider physicsCollider;
   

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        physicsCollider.enabled = false;
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        physicsCollider.enabled = true;
        base.OnSelectExited(args);
    }
}