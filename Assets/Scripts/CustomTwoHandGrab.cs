using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomTwoHandGrab : XRGrabInteractable
{
    [SerializeField] private XRDirectInteractor leftHandInteractor;
    [SerializeField] private XRDirectInteractor rightHandInteractor;
    [SerializeField] private Transform leftHandAttachPoint;
    [SerializeField] private Transform rightHandAttachPoint;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if(args.interactorObject == leftHandInteractor)
        {
            if(this.interactorsSelecting.Count == 1)    // ���������� �̹� ���ͷ��� �����
            {
                
            }
            else                                        // �޼��� ù ���ͷ��� ���
            {
                this.attachTransform.SetPositionAndRotation(leftHandAttachPoint.position, leftHandAttachPoint.rotation);
            }
        }
        else if(args.interactorObject == rightHandInteractor)
        {
            if (this.interactorsSelecting.Count == 1)   // �޼��� �̹� ���ͷ��� �����
            {

            } 
            else                                        // �������� ù ���ͷ��� ���
            {
                this.attachTransform.SetPositionAndRotation(this.rightHandAttachPoint.position, this.rightHandAttachPoint.rotation);
            }
        }
        
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (args.interactorObject == leftHandInteractor)
        {
            // �޼��� �������� �ൿ
            if (this.interactorsSelecting.Count == 2)
            {
                this.attachTransform.SetPositionAndRotation(this.rightHandAttachPoint.position, this.rightHandAttachPoint.rotation);
            }
        }
        else if (args.interactorObject == rightHandInteractor)
        {
            // �������� �������� �ൿ
            if(this.interactorsSelecting.Count == 2)
            {
                this.attachTransform.SetPositionAndRotation(this.leftHandAttachPoint.position, this.leftHandAttachPoint.rotation);
            }
        }

        base.OnSelectExiting(args);
    }
}
