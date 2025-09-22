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
    [SerializeField] private GameObject bulletCasingPrefab;

    protected bool isEmpty = false;

    public GameObject Bullet { get => bulletPrefab; }
    public GameObject Casing { get => bulletCasingPrefab; }

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