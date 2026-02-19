using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MovementComponent
{
    [SerializeField] private float _jumpHeight = 6f;
    [SerializeField] private InputAction jumpAction;
    
    private float _coyoteTime = 0.12f;
    private float _jumpBufferTime = 0.12f;

    private float _coyoteTimer;
    private float _jumpBufferTimer;

    private PlayerGravity _gravity;
    private GroundCheck _groundCheck;

    private void OnEnable() => jumpAction.Enable();
    private void OnDisable() => jumpAction.Disable();

    private void Start()
    {
        _gravity = GetComponent<PlayerGravity>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void Update()
    {
        if (_gravity == null || _groundCheck == null)
            return;

        float dt = Time.deltaTime;

        if (_groundCheck.IsGrounded)
            _coyoteTimer = _coyoteTime;
        else
            _coyoteTimer -= dt;

        if (jumpAction.WasPressedThisFrame())
            _jumpBufferTimer = _jumpBufferTime;
        else
            _jumpBufferTimer -= dt;

        if (_jumpBufferTimer > 0f && (_groundCheck.IsGrounded || _coyoteTimer > 0f))
        {
            DoJump();
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }
    }

    private void DoJump()
    {
        float jumpSpeed = Mathf.Sqrt(2f * _gravity.GravityStrength * _jumpHeight);
        _gravity.Jump(jumpSpeed);
    }
}