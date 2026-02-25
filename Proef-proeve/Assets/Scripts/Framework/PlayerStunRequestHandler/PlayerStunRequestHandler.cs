using UnityEngine;
using UnityEngine.Events;

public class PlayerStunRequestHandler : MonoBehaviour
{
    [SerializeField] private float coolDownTime;
    private CountdownTimer _countDownTimer;

    public UnityEvent OnValidStunRequest = new UnityEvent();

    private void Awake() => _countDownTimer = new CountdownTimer(coolDownTime);

    public void RequestStun()
    {
        if(_countDownTimer.IsTimerActive) return;
        _countDownTimer.StartTimer();
        OnValidStunRequest?.Invoke();
    }

    private void Update()
    {
        if (!_countDownTimer.IsTimerActive) return;

        _countDownTimer.Tick(Time.deltaTime);

        if(!_countDownTimer.IsTimerDone) return;

        _countDownTimer.StopTimer();
    }
}
