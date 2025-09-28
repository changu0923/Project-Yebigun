using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TargetPokeButton : MonoBehaviour
{
    [SerializeField] TargetMovement targetMovement;

    private bool isPushed = false;

    public void OnFowardButtonPushed()
    {
        targetMovement.MoveFoward();
        StartAnimation();
    }

    public void OnBackwardButtonPushed()
    {
        targetMovement.MoveBackward();
        StartAnimation();
    }

    private void StartAnimation()
    {
        if (!isPushed)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isPushed = true;

        Vector3 currentPos = transform.localPosition;
        Vector3 desiredPos = currentPos - new Vector3(0f, 0.1f, 0f);
        float duration = 0.33f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currentPos, desiredPos, t / duration); 
            yield return null;
        }
        transform.localPosition = desiredPos;

        t = 0f;
        while(t<duration)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(desiredPos, currentPos, t / duration);
            yield return null;
        }
        transform.localPosition = currentPos;
        isPushed = false;
    }
}