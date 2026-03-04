using System;
using UnityEngine;

public class JumpState : BaseState
{
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float jumpStateDuration = 0.05f;

    private float _timer;
    private MovementData _movementData;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;

        _movementData.JumpHeight = jumpHeight;
        _movementData.JumpRequested = true;

        _timer = jumpStateDuration;
    }

    public override void StateUpdate(float deltaTime)
    {
        _timer -= deltaTime;
        if (_timer <= 0f)
            _onStateCompleted?.Invoke();
    }

    public override void StateExit() { }
}