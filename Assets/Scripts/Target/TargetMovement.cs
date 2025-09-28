using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    private Animator animator;

    private enum TargetState
    {
        Idle,
        Shooting,
    }

    private TargetState currentState;

    private bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = TargetState.Idle;
        isMoving = false;
    }

    public void MoveFoward()
    {
        if (isMoving)
            return;

        if (currentState == TargetState.Shooting)
            return;

        isMoving = true;
        currentState = TargetState.Shooting;        
        animator.SetTrigger("Foward");
    }

    public void MoveBackward()
    {
        if (isMoving) 
            return;

        if (currentState == TargetState.Idle)
            return;

        isMoving = true;
        currentState = TargetState.Idle;
        animator.SetTrigger("Backward");
    }

    #region AnimationEvents
    public void OnFowardDone()
    {
        isMoving = false;
    }

    public void OnBackwardDone()
    {
        isMoving = false;
    }
    #endregion
}
