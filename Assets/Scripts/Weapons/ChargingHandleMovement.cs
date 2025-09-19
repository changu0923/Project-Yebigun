using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChargingHandleMovement : XRGrabInteractable
{
    [SerializeField] private Transform originTransform;
    public float maxDistance = 0.0085f;
    public float returnToOriginTime = 0.2f;
    
    private Coroutine returnCouroutine;

    [Header("인터랙터 감지 충돌검사")]
    [SerializeField] private HandDetector handDetector;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {

        Debug.Log("Charging Handle grip enabled");

        base.OnSelectEntering(args);

        if (returnCouroutine != null)
        {
            StopAllCoroutines();
            returnCouroutine = null;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        returnCouroutine = StartCoroutine(ReturnToOrigin());
    }

    IEnumerator ReturnToOrigin()
    {
        Vector3 currentPos = transform.position;
        float timer = 0f;

        while(timer < returnToOriginTime)
        {
            timer+= Time.deltaTime;
            transform.localPosition=Vector3.Lerp(currentPos, originTransform.localPosition  , timer/returnToOriginTime);
            yield return null;
        }

        transform.localPosition = originTransform.localPosition;
        returnCouroutine = null;
    }
}
