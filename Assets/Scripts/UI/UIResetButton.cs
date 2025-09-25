using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResetButton : MonoBehaviour
{
    private Button uiResetButton;
    private void Awake()
    {
        uiResetButton = GetComponent<Button>();
    }
}
