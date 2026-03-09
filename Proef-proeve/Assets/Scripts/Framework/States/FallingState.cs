using System;
using UnityEngine;

public class FallingState : BaseState
{
    private MovementData _movementData;
    private GroundCheck _groundCheck;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
    }

    private void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _movementData.MoveInput = Vector2.zero;
    }

    public override void StateUpdate(float deltaTime)
    {
        _movementData.MoveInput = Vector2.zero;

        if (!_groundCheck.IsGrounded) return;
        _onStateCompleted();
    }

    public override void StateExit() { }
}