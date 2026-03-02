using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : BaseState
{
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private InputAction jumpAction;

    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    private float _coyoteTimer;
    private float _jumpBufferTimer;

    private MovementData _movementData;
    private GroundCheck _groundCheck;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable() => jumpAction.Enable();
    private void OnDisable() => jumpAction.Disable();

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;

        _movementData.JumpHeight = jumpHeight;

        // We assume entering JumpState means "jump was pressed"
        _jumpBufferTimer = jumpBufferTime;
        _coyoteTimer = 0f;
    }

    public override void StateUpdate(float deltaTime)
    {
        bool grounded = _groundCheck != null && _groundCheck.IsGrounded;

        if (grounded) _coyoteTimer = coyoteTime;
        else _coyoteTimer -= deltaTime;

        _jumpBufferTimer -= deltaTime;

        if (_jumpBufferTimer > 0f && (grounded || _coyoteTimer > 0f))
        {
            _movementData.JumpRequested = true;
            _onStateCompleted?.Invoke(); // return to default immediately
            return;
        }

        // If we couldn't jump within the buffer window, leave the jump state
        if (_jumpBufferTimer <= 0f)
        {
            _onStateCompleted?.Invoke();
        }
    }

    public override void StateExit() { }
}