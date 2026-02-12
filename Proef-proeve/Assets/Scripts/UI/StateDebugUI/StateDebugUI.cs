using System;
using UnityEngine;
using TMPro;

public class StateDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    private void OnEnable() => StateMachine.OnNewActiveState += UpdateStateDebugUI;
    private void OnDisable() => StateMachine.OnNewActiveState -= UpdateStateDebugUI;

    private void UpdateStateDebugUI(object sender, string stateName)
    {
        debugText.text = stateName;
    }
}
