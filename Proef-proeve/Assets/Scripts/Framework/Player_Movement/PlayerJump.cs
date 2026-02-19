using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MovementComponent
{
    [SerializeField] private float _jumpHeight = 6f;
    [SerializeField] private InputAction jumpAction;

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

        if (_groundCheck.IsGrounded && jumpAction.WasPressedThisFrame())
        {
            float jumpSpeed = Mathf.Sqrt(2f * _gravity.GravityStrength * _jumpHeight);
            _gravity.Jump(jumpSpeed);
        }
    }
}