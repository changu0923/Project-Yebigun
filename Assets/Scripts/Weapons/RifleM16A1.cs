using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private FireMode fireMode = FireMode.Auto;
    private Chamber chamber = Chamber.Closed;

    private bool isTriggerOn = false;
    private bool isBoltCatchOn = false;
    private bool dryFire = false;
    private Coroutine process = null;

    [SerializeField] PlayVFX VFX;
    [SerializeField] PlaySFX SFX;

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
            }
        }
    }
    public void OnMagazineRemoved()
    {
        currentMag = null;
    }
    #endregion

    #region ChargingHandle Event
    public void ChargingHandlePulled()
    {  
        if(isChamberLoaded)
        {
            RemoveBulletFromChamber();
        }
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
            // TODO : 풀링
            GameObject bullet = Instantiate(chamberedBullet, muzzle.position, muzzle.rotation);
            Bullet spawnedBullet = bullet.GetComponent<Bullet>();
            spawnedBullet.Fire();            
            SFX.Play();
            VFX.SpawnVFX();
            ChamberProcessAfterShot();
            return;
        }

        if(chamber == Chamber.Closed && !isChamberLoaded && dryFire)
        {
            // TODO : 공격발 
            Debug.Log("Dry Fire");
            dryFire = false;
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
        GameObject casing = Instantiate(chamberedCasing, ejectionPort.position, ejectionPort.rotation);
        BulletCasing bulletCasing = casing.GetComponent<BulletCasing>();
        bulletCasing.Eject();
        chamberedCasing = null;
    }

    private void EjectBullet()
    {
        GameObject bullet = Instantiate(chamberedBulletFull, ejectionPort.position, ejectionPort.rotation);
        BulletFull fullBullet = bullet.GetComponent<BulletFull>();
        fullBullet.Eject();
        chamberedBulletFull = null;
    }

    public void BoltCatchPushed()
    {
        isBoltCatchOn = false;
    }

    IEnumerator FireCoroutine()
    {
        if (fireMode == FireMode.Safe)
        {
            //TODO : Safe 사운드효과
            Debug.Log("Firemode is Safe");
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