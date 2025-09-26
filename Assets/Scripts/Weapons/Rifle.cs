using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform ejectionPort;
    [SerializeField] protected string ammoType;

    public Action OnShotFired;

    protected Magazine currentMag;

    protected bool isChamberLoaded = false;
}
