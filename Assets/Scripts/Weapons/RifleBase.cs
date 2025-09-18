using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RifleBase : MonoBehaviour
{
    [SerializeField] protected XRSocketInteractor magazineSocket;
    [SerializeField] protected string ammoType;
}
