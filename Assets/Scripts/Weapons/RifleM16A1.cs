using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RifleM16A1 : Rifle
{
    [SerializeField] float RPM;

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

    private bool isTriggerOn = false;
    private bool isBoltCatchOn = false;
    private bool dryFire = false;
    private Coroutine process = null;
    
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
            chamber = Chamber.Closed;
            dryFire = !isChamberLoaded;
        }
        else
        {
            isChamberLoaded = false;
            chamber = Chamber.Closed;
            dryFire = true;
        }
    }

    private void RemoveBulletFromChamber()
    {
        // TODO : 총알 한발 날리기
        isChamberLoaded = false;
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
            GameObject bullet = Instantiate(currentMag.Bullet, muzzle.position, muzzle.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(muzzle.forward * 960f, ForceMode.Impulse);
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

        if (currentMag != null)
        {
            bool result = GetBulletFromMagazine();
            if (result)
            {
                isChamberLoaded = true;
                chamber = Chamber.Closed;
            }
            else
            {
                isChamberLoaded = false;
                isBoltCatchOn = true;
                chamber = Chamber.Locked;
                dryFire = false;
            }
        }
        else
        {
            isChamberLoaded = false;
            chamber = Chamber.Closed;
            dryFire = true;
        }
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