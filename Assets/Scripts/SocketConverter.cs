using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketConverter : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // ��ġ�� �ٲٰ� ���� Ʈ������
    [SerializeField] private Transform desiredTransform; // �ű�� ���� ��ġ
    [SerializeField] private Transform parentTransform; // �θ�

    public void Convert()
    {
        targetTransform.localPosition = parentTransform.InverseTransformPoint(desiredTransform.position);
    }
}
