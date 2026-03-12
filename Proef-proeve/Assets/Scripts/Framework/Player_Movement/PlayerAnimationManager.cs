using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Refs (auto if null)")]
    [SerializeField] private Animator animator;
    [SerializeField] private MovementData movementData;
    [SerializeField] private GroundCheck groundCheck;

    [Header("Speed Normalization")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speedDampTime = 0.08f;

    [Header("Jump / Land")]
    [SerializeField] private float jumpMaxTime = 0.6f;

    [SerializeField] private float landHoldTime = 0.12f;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int Land = Animator.StringToHash("Land");

    private bool _wasGrounded;

    private bool _jumpingActive;
    private bool _leftGroundSinceJump;
    private float _jumpTimer;

    private float _landTimer;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (movementData == null) movementData = GetComponent<MovementData>();
        if (groundCheck == null) groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable()
    {
        _wasGrounded = groundCheck != null && groundCheck.IsGrounded;

        animator.SetBool(Grounded, _wasGrounded);
        animator.SetBool(IsJumping, false);
        animator.SetBool(Land, false);

        _jumpingActive = false;
        _leftGroundSinceJump = false;
        _jumpTimer = 0f;
        _landTimer = 0f;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        animator.SetBool(Grounded, groundCheck.IsGrounded);

        float speed01 = Mathf.InverseLerp(0, 5, movementData.MoveSpeed);
        animator.SetFloat(Speed, speed01, speedDampTime, dt);

        if (movementData != null && movementData.JumpRequested)
        {
            _jumpingActive = true;
            _leftGroundSinceJump = false;
            _jumpTimer = jumpMaxTime;

            _landTimer = 0f;
        }

        if (_jumpingActive && !groundCheck.IsGrounded)
            _leftGroundSinceJump = true;

        if (_jumpTimer > 0f)
            _jumpTimer -= dt;

        bool landedNow = (!_wasGrounded && groundCheck.IsGrounded);
        if (_jumpingActive)
        {
            if ((_leftGroundSinceJump && groundCheck.IsGrounded) || _jumpTimer <= 0f)
            {
                _jumpingActive = false;
                _leftGroundSinceJump = false;
                _jumpTimer = 0f;
            }
        }

        animator.SetBool(IsJumping, _jumpingActive);

        if (landedNow)
            _landTimer = landHoldTime;

        if (_landTimer > 0f)
            _landTimer -= dt;

        animator.SetBool(Land, _landTimer > 0f);

        _wasGrounded = groundCheck.IsGrounded;
    }

    public void NotifyJumpStarted()
    {
        _jumpingActive = true;
        _leftGroundSinceJump = false;
        _jumpTimer = jumpMaxTime;
        _landTimer = 0f;
    }
}