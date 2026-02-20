using System;
using UnityEngine;

public class StunState : BaseState
{
    [SerializeField] private float stunDuration; 
    private CountdownTimer stunCountdownTimer;

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        stunCountdownTimer = new CountdownTimer(stunDuration);
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate(float deltaTime)
    {
        stunCountdownTimer.Tick(deltaTime);

        if (!stunCountdownTimer.IsTimerDone)
        _onStateCompleted();
    }


}
