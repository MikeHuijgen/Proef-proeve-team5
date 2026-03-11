using System;
using UnityEngine;
using TMPro;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oxygenValueText;

    void OnEnable() => OxygenComponent.OnUpdatedOxygen += OnUpdatedOxygen;
    void OnDisable() => OxygenComponent.OnUpdatedOxygen -= OnUpdatedOxygen;

    private void OnUpdatedOxygen(float newOxygenValue) => oxygenValueText.text = $"Oxygen: {newOxygenValue.ToString("F2")}";
}
