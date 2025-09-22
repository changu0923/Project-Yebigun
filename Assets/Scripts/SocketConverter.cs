using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketConverter : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // 위치를 바꾸고 싶은 트랜스폼
    [SerializeField] private Transform desiredTransform; // 옮기고 싶은 위치
    [SerializeField] private Transform parentTransform; // 부모

    public void Convert()
    {
        targetTransform.localPosition = parentTransform.InverseTransformPoint(desiredTransform.position);
    }
}
