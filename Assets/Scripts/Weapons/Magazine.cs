using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magazine : XRGrabInteractable
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] protected Collider physicsCollider;

    #region Collider
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        physicsCollider.enabled = false;
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        physicsCollider.enabled = true;
        base.OnSelectExited(args);
    }
    #endregion

    [SerializeField] protected int maxAmmoCapacity;
    protected int currentAmmo;
    private bool isEmpty = false;

    protected GameObject BulletPrefab { get => bulletPrefab; set => bulletPrefab = value; }
    public bool IsEmpty { get => isEmpty; }

    private void Awake()
    {
        currentAmmo = maxAmmoCapacity;
    }
    public void Use()
    {
        if (currentAmmo <= 0)
        {
            isEmpty = true;
            return;
        }

        currentAmmo--;
    }
}
