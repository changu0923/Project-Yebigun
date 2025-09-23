using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] protected int maxAmmo;
    private int currentAmmo;
    [SerializeField] protected string ammoType;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletCasingPrefab;
    [SerializeField] private GameObject bulletFullPrefab;
    [SerializeField] private MagazineFollower magazineFollower;

    protected bool isEmpty = false;

    public GameObject Bullet { get => bulletPrefab; }
    public GameObject Casing { get => bulletCasingPrefab; }
    public GameObject BulletFull { get => bulletFullPrefab; }
    public int CurrentAmmo { get => currentAmmo; }

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
        magazineFollower.Use();
        return true;
    }
}