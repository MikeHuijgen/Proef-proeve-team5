using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStunRequestHandler : MonoBehaviour
{
    [SerializeField] private float coolDownTime = 2f;
    [SerializeField] private StateIntentData stunIntentReference;
    private CountdownTimer _countDownTimer;
    public event Action<StateIntentData> OnStunRequest;

    private void Start() => _countDownTimer = new CountdownTimer(coolDownTime);

    private void OnEnable() => StunState.OnStunStateExit += StartCountdown;
    private void OnDisable() => StunState.OnStunStateExit -= StartCountdown;

    public void RequestStun()
    {
        if(_countDownTimer.IsTimerActive) return;
        OnStunRequest?.Invoke(stunIntentReference);
    }

    private void Update()
    {
        if (!_countDownTimer.IsTimerActive) return;

        _countDownTimer.Tick(Time.deltaTime);

        if(!_countDownTimer.IsTimerDone) return;

        _countDownTimer.StopTimer();
    }

    private void StartCountdown() => _countDownTimer.StartTimer();
}
