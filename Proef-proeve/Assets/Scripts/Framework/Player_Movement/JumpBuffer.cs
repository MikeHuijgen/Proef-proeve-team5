using System;
using UnityEngine;

public class JumpBuffer : MonoBehaviour
{
    private bool _isJumping = false;
    private bool _bufferActive = false;
    
    private float _coyoteTimer;
    private float _jumpBufferTimer;

    private PlayerGravity _gravity;
    private GroundCheck _groundCheck;
    
    public event Action<StateIntentData> OnConfirmJump;

    [SerializeField] private StateIntentData jumpIntentData;

    private void Start()
    {
        _gravity = GetComponent<PlayerGravity>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnNewJumpInput += OnNewJumpInput;
    }
    
    private void OnDisable()
    {
        InputHandler.Instance.OnNewJumpInput -= OnNewJumpInput;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (_groundCheck.IsGrounded)
            _coyoteTimer = 000000;
        else
            _coyoteTimer -= dt;

        if (_isJumping)
        {
            /*_jumpBufferTimer = _jumpBufferTime;*/
        }
        else
            _jumpBufferTimer -= dt;

        if (_jumpBufferTimer > 0f && (_groundCheck.IsGrounded || _coyoteTimer > 0f))
        {
            
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }
    }
    
    private void OnNewJumpInput()
    {
        if (!_bufferActive) _isJumping = true;
        
        OnConfirmJump?.Invoke(jumpIntentData);
    }
}



