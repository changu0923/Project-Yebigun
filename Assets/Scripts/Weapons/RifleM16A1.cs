using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RifleM16A1 : RifleBase
{
    private bool hasMagazine = false;
    private Magazine currentMag;

    private bool isTriggerActivated = false;
    private bool hasRoundInChamber = false;


    [SerializeField] private float RPM = 800;

    Firemode currentFiremode = Firemode.Safe;
    ChamberStatus chamberMode = ChamberStatus.Closed;
    BoltStatus boltStatus = BoltStatus.Closed;  

    public enum Firemode
    {
        Safe,   // ����
        Semi,   // �ܹ�
        Auto,   // �ڵ�
    }   

    public enum ChamberStatus
    {
        Closed, // ��� ����
        Opened, // ��� ����
    }

    public enum  BoltStatus
    {
        Closed, // �븮�� ����
        Opened, // �븮�� ������
        Locked, // �븮�� ���� ����
    }

    private bool boltCatchEnabled = false;

    public void OnActivate()
    {
        // TODO : Ʈ���� ��ư �������� �ൿ
        TryFire();
    }

    public void OnDeactivate()
    {
        // TODO ; Ʈ���� ��ư ������ �ൿ
        StopAllCoroutines();
    }

    private void TryFire()
    {
        if (currentFiremode == Firemode.Safe)
        {
            // TODO : ��� ���� ȿ���� ���
            return;
        }
        else
        {
            StartCoroutine(FireSelector());
        }
    }

    private IEnumerator FireSelector()
    {
        float roundsPerMinute = 60f / RPM;

        if (currentFiremode == Firemode.Semi)
        {
            //TODO : Shoot() �ѹ� ȣ�� �� �ڷ�ƾ ����
            Shoot();
            yield break;
        }
        else if (currentFiremode == Firemode.Auto)
        {
            //TODO : Shoot() �ѹ� ȣ�� �� ���� RPM ��ŭ ����� ���� ���
            while (isTriggerActivated)
            {
                Shoot();
                yield return new WaitForSeconds(roundsPerMinute);
            }           
        }        
    }

    private void Shoot()
    {
        if (hasRoundInChamber && boltStatus == BoltStatus.Closed)
        {
            // TODO �Ѿ� �߻� ����
            Debug.Log("Fire");

            if (!currentMag.IsEmpty && currentMag != null)
            {
                hasRoundInChamber = true;
                currentMag.Use();
            }
            else
            {
                boltCatchEnabled = true;        // �븮�� ��ġ �۵�
                boltStatus = BoltStatus.Locked; // �븮�� ����
            }
            return;
        }
    }
     

    public void OnChargingHandlePulled()
    {
        // TODO: ���������� ����
        if(hasRoundInChamber)
        {
            EjectChamberRound();
            hasRoundInChamber = false;
        }

        if (!currentMag.IsEmpty && currentMag != null)
        {
            currentMag.Use();
            hasRoundInChamber = true;
            boltStatus = BoltStatus.Closed;
        }
        else
        {
            hasRoundInChamber = false;
            boltStatus = BoltStatus.Locked;
        }        
    }



    private void EjectChamberRound()
    {
        // TODO : ��� ����
    }

    public void OnBoltCatchInteract()
    {
        if(hasMagazine &&  boltStatus == BoltStatus.Locked)
        {
            currentMag.Use();
            boltStatus = BoltStatus.Closed;
        }

        boltCatchEnabled = false;
    }

    public void OnSocketAttached(SelectEnterEventArgs args)
    {
        // TODO : źâ ������ �ൿ
        currentMag = args.interactableObject.transform.GetComponent<Magazine>();
        hasMagazine = true;
    }

    public void OnSocketDettatched(SelectEnterEventArgs args)
    {
        // TODO : źâ �и��� �ൿ
        currentMag = null;
        hasMagazine = false;
    }
}
