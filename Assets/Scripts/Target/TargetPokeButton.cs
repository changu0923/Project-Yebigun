using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPokeButton : MonoBehaviour
{
    [SerializeField] TargetMovement targetMovement;
    public void OnFowardButtonPushed()
    {
        targetMovement.MoveFoward();
    }

    public void OnBackwardButtonPushed()
    {
        targetMovement.MoveBackward();
    }
}
