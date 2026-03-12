using System;
using UnityEngine;

public class IdleState : BaseState
{
    private MovementData _movementData;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;

        _movementData.MoveInput = Vector2.zero;
        _movementData.MoveSpeed = 0f;
    }

    public override void StateUpdate(float deltaTime)
    {
        _movementData.MoveInput = Vector2.zero;
        _movementData.MoveSpeed = 0f;
    }

    public override void StateExit() { }
}