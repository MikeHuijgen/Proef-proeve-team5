using System;
using UnityEngine;
using TMPro;

public class StateDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    private void OnEnable() => StateMachine.OnNewActiveState += UpdateStateDebugUI;
    private void OnDisable() => StateMachine.OnNewActiveState -= UpdateStateDebugUI;

    private void UpdateStateDebugUI(string stateName)
    {
        debugText.text = stateName;
    }
}
