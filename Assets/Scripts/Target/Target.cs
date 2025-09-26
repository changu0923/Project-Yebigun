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

    public void OnImpact(GameObject gameObject)
    {
        impacts.Add(gameObject);
        gameObject.transform.SetParent(listTransform);
        currentScore++;
        CalculateHitRatio();
    }    
}
