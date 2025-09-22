using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineInteractor : XRGrabInteractable
{
    [SerializeField] Collider physicsCollider;
    [SerializeField] GameObject leftHandPresent;
    [SerializeField] Transform socketAttachTransform;
    [SerializeField] Transform handAttachTransform;

    public Transform SocketAttachTransform { get => socketAttachTransform; }

    public void HandPresent(bool bValue)
    {
        leftHandPresent.SetActive(bValue);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        physicsCollider.enabled = false;

        if (args.interactorObject is XRSocketInteractor socket)     // 소켓이면 손 숨기기
        {
            HandPresent(false);
        }
        else
        {
            HandPresent(true);
        }
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        physicsCollider.enabled = true;

        if (args.interactorObject is XRSocketInteractor socket) // 소켓에서 뺀거면 손 보이기
        {
            HandPresent(true);
        }
        else
        {
            HandPresent(false);
        }
        base.OnSelectExited(args);
    }
}