using System;
using UnityEngine;

public class StunState : BaseState
{
    [SerializeField] private float stunDuration;

    private CountdownTimer _stunCountdownTimer;

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _stunCountdownTimer = new CountdownTimer(stunDuration);
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate(float deltaTime)
    {
        _stunCountdownTimer.Tick(deltaTime);

        if (!_stunCountdownTimer.IsTimerDone)
        _onStateCompleted();
    }


}
