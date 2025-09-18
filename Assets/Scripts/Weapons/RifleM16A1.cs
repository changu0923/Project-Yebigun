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
        Safe,   // 안전
        Semi,   // 단발
        Auto,   // 자동
    }   

    public enum ChamberStatus
    {
        Closed, // 약실 밀폐
        Opened, // 약실 개방
    }

    public enum  BoltStatus
    {
        Closed, // 노리쇠 닫힘
        Opened, // 노리쇠 조작중
        Locked, // 노리쇠 후퇴 고정
    }

    private bool boltCatchEnabled = false;

    public void OnActivate()
    {
        // TODO : 트리거 버튼 눌럿을때 행동
        TryFire();
    }

    public void OnDeactivate()
    {
        // TODO ; 트리거 버튼 뗏을때 행동
        StopAllCoroutines();
    }

    private void TryFire()
    {
        if (currentFiremode == Firemode.Safe)
        {
            // TODO : 사격 실패 효과음 재생
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
            //TODO : Shoot() 한번 호출 후 코루틴 종료
            Shoot();
            yield break;
        }
        else if (currentFiremode == Firemode.Auto)
        {
            //TODO : Shoot() 한번 호출 후 현재 RPM 만큼 대기후 다음 사격
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
            // TODO 총알 발사 구현
            Debug.Log("Fire");

            if (!currentMag.IsEmpty && currentMag != null)
            {
                hasRoundInChamber = true;
                currentMag.Use();
            }
            else
            {
                boltCatchEnabled = true;        // 노리쇠 멈치 작동
                boltStatus = BoltStatus.Locked; // 노리쇠 개방
            }
            return;
        }
    }
     

    public void OnChargingHandlePulled()
    {
        // TODO: 장전손잡이 구현
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
        // TODO : 약실 비우기
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
        // TODO : 탄창 결합후 행동
        currentMag = args.interactableObject.transform.GetComponent<Magazine>();
        hasMagazine = true;
    }

    public void OnSocketDettatched(SelectEnterEventArgs args)
    {
        // TODO : 탄창 분리후 행동
        currentMag = null;
        hasMagazine = false;
    }
}
