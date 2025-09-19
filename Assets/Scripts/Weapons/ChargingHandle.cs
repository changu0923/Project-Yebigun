using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChargingHandle : XRSimpleInteractable
{
    [SerializeField] private Animator animator;
    private XRSimpleInteractable chargingHandInteractable;

    private bool isSelecting = false;

    protected override void Awake()
    {
        base.Awake();
        chargingHandInteractable = GetComponent<XRSimpleInteractable>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        isSelecting = true;
        PlayAnimation();
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        isSelecting = false;
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        animator.ResetTrigger("DoAction");
        animator.SetBool("isSelecting", isSelecting);
        animator.SetTrigger("DoAction");
    }
}
