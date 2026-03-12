using System;
using UnityEngine;
using TMPro;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    void OnEnable() => OxygenComponent.OnUpdatedOxygen += OnUpdatedOxygen;
    void OnDisable() => OxygenComponent.OnUpdatedOxygen -= OnUpdatedOxygen;

    private void OnUpdatedOxygen(float newOxygenValue)
    {
        var inverted = 100f - newOxygenValue;

        skinnedMeshRenderer.SetBlendShapeWeight(0, inverted);
    }
}
