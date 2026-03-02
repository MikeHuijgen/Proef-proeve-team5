using System;
using UnityEngine;

public class MoveState : BaseState
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stopThreshold = 0.1f;

    private MovementData _movementData;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _movementData.MoveSpeed = moveSpeed;
        _movementData.RotationSpeed = rotationSpeed;
    }

    public override void StateUpdate(float deltaTime)
    {
        Vector2 input = InputHandler.Instance.GetMoveValue();
        _movementData.MoveInput = input;

        // Optional: keep these set while in the state
        _movementData.MoveSpeed = moveSpeed;
        _movementData.RotationSpeed = rotationSpeed;

        // Only exit MoveState when player stops moving
        if (input.magnitude <= stopThreshold)
        {
            _onStateCompleted?.Invoke(); // returns to defaultState (likely Idle)
        }
    }

    public override void StateExit()
    {
        _movementData.MoveInput = Vector2.zero;
    }
}