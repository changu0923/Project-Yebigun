using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrenadeInteractor : XRGrabInteractable
{
    [SerializeField] private Collider physicsCollider;
    [SerializeField] Transform handPresent;
    private Grenade grenade;

    protected override void Awake()
    {  
        base.Awake(); 
        grenade = GetComponent<Grenade>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        physicsCollider.enabled = false;
        handPresent.gameObject.SetActive(true);
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        physicsCollider.enabled = true;
        handPresent.gameObject.SetActive(false);
        base.OnSelectExited(args);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
    }

}
