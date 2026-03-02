using System;
using UnityEngine;

public class OxygenStation : MonoBehaviour
{
    [SerializeField] private float refillAmountPerSecond = 3f;
    private OxygenComponent _oxygenComponent;

    private void Update()
    {
        if (_oxygenComponent == null) return;
        _oxygenComponent.RefillOxygenByAmount(refillAmountPerSecond * Time.deltaTime);
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<OxygenComponent>(out _oxygenComponent)) return;
    }

    private void OnTriggerExit(Collider other)
    {
        _oxygenComponent.ExitOxygenStation();
        _oxygenComponent = null;
    }
}
