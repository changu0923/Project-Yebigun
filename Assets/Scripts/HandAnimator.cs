using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HandAnimator : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    private Animator handAnimator;

    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        float trigerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", trigerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}