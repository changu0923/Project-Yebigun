using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private List<GameObject> impacts = new List<GameObject>();
    private int currentScore = 0;
    private int currentShoot = 0;
    private float hitRatio;
    [SerializeField] Transform listTransform;
    [SerializeField] Rifle rifle; 

    public Action OnScoreChanged;
    public int CurrentScore { get => currentScore; }
    public float HitRatio { get => hitRatio; }

    private void Awake()
    {
        rifle.OnShotFired += OnShootCalled;
    }

    private void CalculateHitRatio()
    {
        hitRatio = ((float)currentScore / (float)currentShoot);
        OnScoreChanged?.Invoke();
    }

    private void OnShootCalled()
    {
        currentShoot++;
        CalculateHitRatio();
    }

    public void OnResetCalled()
    {
        currentScore = 0;
        currentShoot = 0;
        hitRatio = 0f;

        foreach (GameObject impact in impacts)
        {
            if (impact != null)
            {
                ObjectPoolManager.Instance.Destroy("BulletHole", impact);
            }
        }
        impacts.Clear();
    }

    public void OnImpact(GameObject gameObject)
    {
        impacts.Add(gameObject);
        gameObject.transform.SetParent(listTransform);

        Vector3 pos = gameObject.transform.localPosition;
        pos.z = -0.001f;
        gameObject.transform.localPosition = pos;

        currentScore++;
        CalculateHitRatio();
    }    
}
