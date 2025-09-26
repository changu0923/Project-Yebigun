using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHitInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitText;
    [SerializeField] private TextMeshProUGUI hitRatioText;
    [SerializeField] private Button uiResetButton;
    [SerializeField] private Target target;

    private void Awake()
    {
        ResetUI();
        uiResetButton.onClick.AddListener(OnButtonClicked);
        target.OnScoreChanged += UpdateUI;
    }    

    private void OnButtonClicked()
    {
        ResetUI();
    }

    private void UpdateUI()
    {
        hitText.text = $"Hit : {target.CurrentScore}";
        hitRatioText.text = $"Accuracy : {(target.HitRatio*100f):F0}%";
    }

    private void ResetUI()
    {
        hitText.text = "Hit : 0";
        hitRatioText.text = "Accuracy : 0%";
    }
}