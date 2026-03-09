using System;
using UnityEngine;

[RequireComponent(typeof(MovementData))]
[RequireComponent(typeof(GroundCheck))]
public class JumpBuffer : MonoBehaviour
{
    private MovementData _movementData;
    private GroundCheck _groundCheck;

    private float _coyoteTimer;
    private float _bufferTimer;

    public event Action<StateIntentData> OnConfirmJump;
    [SerializeField] private StateIntentData jumpIntentData;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnNewJumpInput += OnNewJumpInput;
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnNewJumpInput -= OnNewJumpInput;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (_groundCheck != null && _groundCheck.IsGrounded)
        {
            _coyoteTimer = _movementData != null ? _movementData.CoyoteTime : 0f;
        }
        else
        {
            _coyoteTimer -= dt;
        }

        if (_bufferTimer > 0f)
            _bufferTimer -= dt;

        bool canJumpNow = (_groundCheck != null && _groundCheck.IsGrounded) || _coyoteTimer > 0f;

        if (_bufferTimer > 0f && canJumpNow)
        {
            if (_movementData != null)
                _movementData.JumpRequested = true;

            _bufferTimer = 0f;
            _coyoteTimer = 0f;
        }
    }

    private void OnNewJumpInput()
    {
        if (_movementData != null)
            _bufferTimer = _movementData.JumpBufferTime;
        else
            _bufferTimer = 0.12f;

        OnConfirmJump?.Invoke(jumpIntentData);
    }

    public void QueueJump()
    {
        OnNewJumpInput();
    }
}