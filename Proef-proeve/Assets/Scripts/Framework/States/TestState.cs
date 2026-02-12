using System;
using UnityEngine;

public class TestState : BaseState
{
    [SerializeField] private float timerDuration = 2f;
    private float _currentTimeDuration;

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
    }

    public override void StateExit()
    {
        _currentTimeDuration = 0f;
    }

    public override void StateUpdate(float deltaTime)
    {
        Timer(deltaTime);
    }

    private void Timer(float deltaTime)
    {
        if (_currentTimeDuration < timerDuration)
        {
            _currentTimeDuration += deltaTime;
        }
        else
        {
            _onStateCompleted();
        }
    }
}
