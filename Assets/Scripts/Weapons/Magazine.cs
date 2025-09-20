using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] protected int maxAmmo;
    protected int currentAmmo;
    [SerializeField] protected string ammoType;
    [SerializeField] private GameObject bulletPrefab;

    protected bool isEmpty = false;

    public GameObject Bullet { get => bulletPrefab; }

    protected void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public bool Use()
    {
        if (currentAmmo <= 0)
        {
            isEmpty = true;
            return false;
        }

        currentAmmo--;
        return true;
    }
}