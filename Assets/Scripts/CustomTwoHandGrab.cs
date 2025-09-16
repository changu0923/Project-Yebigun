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
            if(this.interactorsSelecting.Count == 1)    // 오른손으로 이미 인터랙터 사용중
            {
                
            }
            else                                        // 왼손이 첫 인터랙터 사용
            {
                this.attachTransform.SetPositionAndRotation(leftHandAttachPoint.position, leftHandAttachPoint.rotation);
            }
        }
        else if(args.interactorObject == rightHandInteractor)
        {
            if (this.interactorsSelecting.Count == 1)   // 왼손이 이미 인터랙터 사용중
            {

            } 
            else                                        // 오른손이 첫 인터랙터 사용
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
            // 왼손을 놓았을때 행동
            if (this.interactorsSelecting.Count == 2)
            {
                this.attachTransform.SetPositionAndRotation(this.rightHandAttachPoint.position, this.rightHandAttachPoint.rotation);
            }
        }
        else if (args.interactorObject == rightHandInteractor)
        {
            // 오른손을 놓았을때 행동
            if(this.interactorsSelecting.Count == 2)
            {
                this.attachTransform.SetPositionAndRotation(this.leftHandAttachPoint.position, this.leftHandAttachPoint.rotation);
            }
        }

        base.OnSelectExiting(args);
    }
}
