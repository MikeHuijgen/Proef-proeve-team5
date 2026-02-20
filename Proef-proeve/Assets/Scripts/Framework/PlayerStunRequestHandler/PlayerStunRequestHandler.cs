using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStunRequestHandler : MonoBehaviour
{
    [SerializeField] private float coolDownTime;
    private CountdownTimer _countDownTimer;

    public UnityEvent OnValidStunRequest = new UnityEvent();

    private void OnEnable() => StunComponent.OnPlayerCollision += HandleStunRequest;
    private void OnDisable() => StunComponent.OnPlayerCollision -= HandleStunRequest;

    private void Awake() => _countDownTimer = new CountdownTimer(coolDownTime);

    private void HandleStunRequest(StateIntentData data)
    {
        if(!_countDownTimer.IsTimerDone) return;

        //Hier de stun laten werken
        // Hier ook checken of hij wel mag stunnen dan het event afvuren waar de statemachine naar luister
    }

    private void Update()
    {
        // Dat de counter niet doorgaat als hij klaar is en return
        // en dan dus weer de stun allow

        _countDownTimer.Tick(Time.deltaTime);
    }
}
