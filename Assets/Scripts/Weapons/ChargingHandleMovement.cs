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
    
    private Transform currentInteractor = null;
    private bool isGrabbed = false;
    private Coroutine returnCouroutine;

    [Header("인터랙터 감지 충돌검사")]
    [SerializeField] private HandDetector handDetector;
    private bool isPriority = false;

    protected override void Awake()
    {
        base.Awake();
        if(handDetector != null )        {            handDetector.OnHandDetected += HandDetected;        }
    }

    protected override void OnDisable()
    {
        if (handDetector != null) { handDetector.OnHandDetected -= HandDetected; }
    }

    private void Update()
    {
        if(isGrabbed)
        {
            MoveHandle();
        }
    }

    private void HandDetected(XRDirectInteractor interactor, bool isHovering)
    {
        if (isHovering)
        {
            Debug.Log($"{interactor.transform.name} is Hovering");
        }
        else
        {
            Debug.Log($"{interactor.transform.name} is Out");
        }
    }

    private void MoveHandle()
    {
        Vector3 worldHandPos = currentInteractor.transform.position;
        Vector3 localHandPos = originTransform.parent.InverseTransformPoint(worldHandPos);
        Vector3 currentOriginPos = originTransform.localPosition;
        float zMovement = Vector3.Dot(localHandPos-currentOriginPos, originTransform.forward);
        float zClamp = Mathf.Clamp(zMovement, -0.0085f, 0f);
        Vector3 newLocalPos = currentOriginPos + originTransform.forward * zClamp;
        originTransform.localPosition = newLocalPos;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (returnCouroutine != null)
        {
            StopAllCoroutines();
            returnCouroutine = null;
        }

        if (args.interactorObject is XRDirectInteractor directInteractor)
        {
            currentInteractor = directInteractor.transform;
        }

        isGrabbed = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        returnCouroutine = StartCoroutine(ReturnToOrigin());
        isGrabbed = false;
        currentInteractor = null;        
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
