using System;
using UnityEngine;
using UnityEngine.Events;

public class OxygenComponent : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenDecreaseAmount = 1f;
    public UnityEvent OnNoOxygenLeft = new UnityEvent();
    public static event Action<float> OnUpdatedOxygen;

    private float _currentOxygen;
    private bool _isRefillingOxygen;
    private bool _oxygenIsMaxed;
    private bool _allowedTheDecrease = true;

    private void Awake() => ResetOxygen();
    private void Update() => DecreaseOxygen();

    private void OnEnable() => PlayerDeath.OnDie += ResetOxygen;
    private void OnDisable() => PlayerDeath.OnDie -= ResetOxygen;

    public void RefillOxygenByAmount(float amount)
    {
        if (_oxygenIsMaxed) return;
        _isRefillingOxygen = true;
        _currentOxygen += amount;
        OnUpdatedOxygen?.Invoke(_currentOxygen);

        if (_currentOxygen < maxOxygen) return;
        _oxygenIsMaxed = true;
        _currentOxygen = maxOxygen;
    }

    private void DecreaseOxygen()
    {
        if (_isRefillingOxygen || _currentOxygen <= 0 || !_allowedTheDecrease) return;
        _oxygenIsMaxed = false;
        _currentOxygen -= oxygenDecreaseAmount * Time.deltaTime;
        OnUpdatedOxygen?.Invoke(_currentOxygen);

        if (_currentOxygen > 0) return;
        _currentOxygen = 0f;
        OnNoOxygenLeft?.Invoke();
    }

    private void ResetOxygen()
    {
        _currentOxygen = maxOxygen;
        _isRefillingOxygen = false;
        _oxygenIsMaxed = true;
        OnUpdatedOxygen?.Invoke(_currentOxygen);
    }

    public void ExitOxygenStation() => _isRefillingOxygen = false;
    public void IsAllowedToDecrease(bool value) => _allowedTheDecrease = value;
    private void OnGamePaused() => _allowedTheDecrease = false;
    private void OnGameResume() => _allowedTheDecrease = true;
}
