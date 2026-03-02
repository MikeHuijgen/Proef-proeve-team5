using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStunRequestHandler : MonoBehaviour
{
    [SerializeField] private float coolDownTime;
    [SerializeField] private StateIntentData stunIntentReference;
    private CountdownTimer _countDownTimer;
    public event Action<StateIntentData> OnStunRequest;

    private void Awake() => _countDownTimer = new CountdownTimer(coolDownTime);

    public void RequestStun()
    {
        if(_countDownTimer.IsTimerActive) return;
        _countDownTimer.StartTimer();
        OnStunRequest?.Invoke(stunIntentReference);
    }

    private void Update()
    {
        if (!_countDownTimer.IsTimerActive) return;

        _countDownTimer.Tick(Time.deltaTime);

        if(!_countDownTimer.IsTimerDone) return;

        _countDownTimer.StopTimer();
    }
}
