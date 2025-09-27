using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RifleM16A1 : Rifle
{
    [SerializeField] float RPM;
    [SerializeField] Animator chamberAnimator;

    private GameObject chamberedBullet;
    private GameObject chamberedCasing;
    private GameObject chamberedBulletFull;

    public enum FireMode
    {
        Safe,
        Semi,
        Auto,
    }
    public enum Chamber
    {
        Closed,
        Opened,
        Locked,
    }

    private FireMode fireMode = FireMode.Safe;
    private Chamber chamber = Chamber.Closed;

    private float[] selectorAngle = new float[3] {90f, 0f, -90f};
    [SerializeField] Transform selectorTransform;


    private bool isTriggerOn = false;
    private bool isBoltCatchOn = false;
    private bool dryFire = false;
    private Coroutine process = null;
    
    [SerializeField] PlayVFX VFX;

    [Header("SFX")]
    [SerializeField] PlaySFX audioShoot;
    [SerializeField] PlaySFX audioDryfire;
    [SerializeField] PlaySFX audioSelector;
    [SerializeField] PlaySFX audioBoltCatch;
    [SerializeField] PlaySFX audioChargingHandlePull;
    [SerializeField] PlaySFX audioChargingHandleRelease;
    [SerializeField] PlaySFX audioMagazineInsert;
    [SerializeField] PlaySFX audioMagazineEject;

    private void ShotsFired()
    {
        audioShoot.Play();
        VFX.SpawnVFX();
        OnShotFired?.Invoke();
    }

    #region Trigger Event
    public void OnTriggerButtonEnabled()
    {
        if (process != null)
            return;

        isTriggerOn = true;
        process = StartCoroutine(FireCoroutine());
    }

    public void OnTriggerButtonDisabled()
    {
        StopAllCoroutines();
        isTriggerOn = false;
        process = null;
    }
    #endregion

    #region Socket Event
    public void OnMagazineInserted(SelectEnterEventArgs args)
    {
        IXRSelectInteractable socketItem = args.interactableObject;
        if (socketItem != null )
        {
            GameObject obj = socketItem.transform.gameObject;
            bool getMag = obj.TryGetComponent<Magazine>(out Magazine mag);
            if(getMag)
            {
                currentMag = mag;
                audioMagazineInsert.Play();
            }
        }
    }
    public void OnMagazineRemoved()
    {
        currentMag = null;
        audioMagazineEject.Play();
    }
    #endregion

    #region ChargingHandle Event
    public void ChargingHandlePulled()
    {  
        if(isChamberLoaded)
        {
            RemoveBulletFromChamber();
        }
        audioChargingHandlePull.Play();
    }

    public void ChargingHandleReleased()
    {
        if (currentMag != null)
        {
            isChamberLoaded = GetBulletFromMagazine();
            chamberedBullet = currentMag.Bullet;
            chamberedCasing = currentMag.Casing;
            chamberedBulletFull = currentMag.BulletFull;
            chamber = Chamber.Closed;
            dryFire = !isChamberLoaded;
        }
        else
        {
            isChamberLoaded = false;
            chamber = Chamber.Closed;
            dryFire = true;
        }
        chamberAnimator.SetTrigger("Closed");
        audioChargingHandleRelease.Play();
    }
    #endregion

    private void RemoveBulletFromChamber()
    {
        EjectBullet();
        isChamberLoaded = false;
        chamberedCasing = null;
        chamberedBullet = null;
    }

    private bool GetBulletFromMagazine()
    {
        bool result = currentMag.Use();
        return result;
    }

    private void Shoot()
    {
        if (chamber == Chamber.Closed && isChamberLoaded)
        {            
            GameObject bullet = ObjectPoolManager.Instance.Instantiate("Bullet", chamberedBullet);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;

            Bullet spawnedBullet = bullet.GetComponent<Bullet>();
            spawnedBullet.Fire();
            ChamberProcessAfterShot();
            ShotsFired();
            return;
        }

        if(chamber == Chamber.Closed && !isChamberLoaded && dryFire)
        {  
            dryFire = false;
            audioDryfire.Play();
        }
    }

    private void ChamberProcessAfterShot()
    {
        chamber = Chamber.Opened;
        chamberedBullet = null;
        EjectCasing();
        chamberAnimator.SetTrigger("Opened");

        if (currentMag != null)
        {
            bool result = GetBulletFromMagazine();
            if (result)
            {
                isChamberLoaded = true;
                chamberedBullet = currentMag.Bullet;
                chamberedCasing = currentMag.Casing;
                chamberedBulletFull = currentMag.BulletFull;
                chamber = Chamber.Closed;
                chamberAnimator.SetTrigger("Closed");
            }
            else
            {
                isChamberLoaded = false;
                isBoltCatchOn = true;
                chamber = Chamber.Locked;
                dryFire = false;
                chamberAnimator.SetTrigger("Locked");
            }
        }
        else
        {
            isChamberLoaded = false;
            chamber = Chamber.Closed;
            chamberAnimator.SetTrigger("Closed");
            dryFire = false;
        }
    }

    private void EjectCasing()
    {
        GameObject casing = ObjectPoolManager.Instance.Instantiate("Casing", chamberedCasing);
        casing.transform.position = ejectionPort.position;
        casing.transform.rotation = ejectionPort.rotation;
        BulletCasing bulletCasing = casing.GetComponent<BulletCasing>();
        bulletCasing.Eject();
        chamberedCasing = null;
    }

    private void EjectBullet()
    {       
        GameObject bullet = ObjectPoolManager.Instance.Instantiate("BulletFull", chamberedBulletFull);
        bullet.transform.position = ejectionPort.position;
        bullet.transform.rotation = ejectionPort.rotation;
        BulletFull fullBullet = bullet.GetComponent<BulletFull>();
        fullBullet.Eject();
        chamberedBulletFull = null;
    }

    public void BoltCatchPushed()
    {
        Debug.Log("BoltCatchPushed()");

        if (!isBoltCatchOn)
            return;
        else
        {
            ChargingHandleReleased();
            isBoltCatchOn = false;
        }
        audioBoltCatch.Play();
    }

    public void ChangeFireSelector()
    {
        if (isTriggerOn)
            return;

        int currentMode = (int)fireMode;
        int nextMode = (currentMode + 1) % 3;
        fireMode = (FireMode)nextMode;
        RotateSelectorAngle();
        audioSelector.Play();
    }

    private void RotateSelectorAngle()
    {
        int currentFiremode = (int)fireMode;
        Vector3 desiredRotation = new Vector3(selectorAngle[currentFiremode], 0, -180);
        selectorTransform.localEulerAngles = desiredRotation;        
    }

    IEnumerator FireCoroutine()
    {
        if (fireMode == FireMode.Safe)
        {
            yield break;
        }

        if (fireMode == FireMode.Semi)
        {
            Shoot();
            yield break;
        }

        if (fireMode == FireMode.Auto)
        {
            float roundperminute = 60f / RPM;

            while (isTriggerOn)
            {
                Shoot();
                yield return new WaitForSeconds(roundperminute);
            }
        }
    }
}