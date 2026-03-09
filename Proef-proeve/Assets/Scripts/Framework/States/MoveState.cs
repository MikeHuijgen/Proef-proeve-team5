using System;
using UnityEngine;

public class MoveState : BaseState
{
    [Header("Ground")]
    [SerializeField] private float groundMoveSpeed = 5f;
    [SerializeField] private float groundRotationSpeed = 10f;

    [Header("Air")]
    [SerializeField] private float airMoveSpeed = 3.5f;
    [SerializeField] private float airRotationSpeed = 8f;

    [SerializeField] private float inputDeadzone = 0.05f;

    private MovementData _movementData;
    private GroundCheck _groundCheck;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
    }

    public override void StateUpdate(float deltaTime)
    {
        Vector2 input = InputHandler.Instance != null ? InputHandler.Instance.GetMoveValue() : Vector2.zero;
        if (input.magnitude < inputDeadzone) input = Vector2.zero;

        _movementData.MoveInput = input;

        bool grounded = _groundCheck != null && _groundCheck.IsGrounded;

        if (grounded)
        {
            _movementData.MoveSpeed = groundMoveSpeed;
            _movementData.RotationSpeed = groundRotationSpeed;
        }
        else
        {
            _movementData.MoveSpeed = airMoveSpeed;
            _movementData.RotationSpeed = airRotationSpeed;
        }
        
        if (input.magnitude <= 0.05) _onStateCompleted?.Invoke();
        
    }

    public override void StateExit()
    {
        
    }
}