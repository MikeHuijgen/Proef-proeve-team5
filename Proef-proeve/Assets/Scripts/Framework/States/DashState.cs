using System;
using UnityEngine;

public class DashState : BaseState
{
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float inputDeadzone = 0.1f;

    [Header("Optional")]
    [SerializeField] private bool allowAirDash = true;
    [SerializeField] private bool dashForwardWhenNoInput = true;
    [SerializeField] private float dashRotationSpeed = 20f;

    private MovementData _movementData;
    private GroundCheck _groundCheck;
    private Animator _playerAnimator;

    private float _dashTimer;
    private Vector2 _dashInput;
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
        _groundCheck = GetComponent<GroundCheck>();
        _playerAnimator = GetComponent<PlayerAnimationManager>().animator;
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _dashTimer = dashDuration;

        bool grounded = _groundCheck.IsGrounded;
        if (!grounded && !allowAirDash)
        {
            _onStateCompleted?.Invoke();
            return;
        }

        Vector2 moveInput = InputHandler.Instance.GetMoveValue();

        if (moveInput.magnitude < inputDeadzone)
        {
            _dashInput = _movementData.PlayerBody.transform.forward;
        }
        else
        {
            _dashInput = moveInput.normalized;
        }

        _movementData.MoveInput = _dashInput;
        _movementData.MoveSpeed = dashSpeed;
        _movementData.RotationSpeed = dashRotationSpeed;
    }

    public override void StateUpdate(float deltaTime)
    {
        _playerAnimator.SetBool(IsDashing, true);

        _dashTimer -= deltaTime;

        _movementData.MoveInput = _dashInput;
        _movementData.MoveSpeed = dashSpeed;
        _movementData.RotationSpeed = dashRotationSpeed;

        if (_dashTimer <= 0f)
        {
            _playerAnimator.SetBool(IsDashing, false);
            _onStateCompleted?.Invoke();
        }
    }

    public override void StateExit()
    {
        _movementData.MoveInput = Vector2.zero;
    }
}