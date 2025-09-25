using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private List<GameObject> impacts = new List<GameObject>();
    private int currentScore = 0;

    public void OnImpact(GameObject gameObject)
    {
        impacts.Add(gameObject);
        currentScore++;
    }
}
