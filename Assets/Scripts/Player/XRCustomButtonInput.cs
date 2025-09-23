using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCustomButtonInput : MonoBehaviour
{
    public InputActionReference rightHandPrimaryButton;
    public InputActionReference rightHandSecondaryButton;

    public UnityEvent onRightHandPrimaryButtonPressed;
    public UnityEvent onRightHandSecondaryButtonPressed;

    private XRBaseInteractable interatable;

    private void Awake()
    {
        interatable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        interatable.selectEntered.AddListener(OnSelectEntered);
        interatable.selectExited.AddListener(OnSelectExited);

    }

    private void OnDisable()
    {
        interatable.selectEntered.RemoveListener(OnSelectEntered);
        interatable.selectExited.RemoveListener(OnSelectExited); 
    }

    private void OnRightHandPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        onRightHandPrimaryButtonPressed?.Invoke();
    }
    private void OnRightHandSecondaryButtonPressed(InputAction.CallbackContext context)
    {
        onRightHandSecondaryButtonPressed?.Invoke();
    }
    
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(rightHandPrimaryButton !=null)
        {
            rightHandPrimaryButton.action.Enable();
            rightHandPrimaryButton.action.performed += OnRightHandPrimaryButtonPressed;
        }
        if (rightHandSecondaryButton != null)
        {
            rightHandSecondaryButton.action.Enable();
            rightHandSecondaryButton.action.performed += OnRightHandSecondaryButtonPressed;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (rightHandPrimaryButton != null)
        {
            rightHandPrimaryButton.action.performed -= OnRightHandPrimaryButtonPressed;
            rightHandPrimaryButton.action.Disable();
        }
        if (rightHandSecondaryButton != null)
        {
            rightHandSecondaryButton.action.performed -= OnRightHandSecondaryButtonPressed;
            rightHandSecondaryButton.action.Disable();
        }
    }
}