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
    [Tooltip("Maximum time IsJumping can remain true if we never observe airborne/landing transitions.")]
    [SerializeField] private float jumpMaxTime = 0.6f;

    [Tooltip("How long Land stays true after landing (seconds).")]
    [SerializeField] private float landHoldTime = 0.12f;

    // Animator params
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int Land = Animator.StringToHash("Land"); // BOOL now

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

        // Initialize animator values
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

        bool grounded = groundCheck != null && groundCheck.IsGrounded;

        // --- Grounded bool ---
        animator.SetBool(Grounded, grounded);

        // --- Speed 0..1 ---
        float speed01 = ComputePlanarSpeed01Rounded3();
        animator.SetFloat(Speed, speed01, speedDampTime, dt);

        // --- Detect jump start (without StateMachine) ---
        // JumpRequested is set by JumpBuffer and consumed by MovementMotor.
        // We treat observing it as "jump initiated".
        if (movementData != null && movementData.JumpRequested)
        {
            _jumpingActive = true;
            _leftGroundSinceJump = false;
            _jumpTimer = jumpMaxTime;

            // When starting a jump, clear Land
            _landTimer = 0f;
        }

        // If we are in a jump sequence, observe leaving the ground at least once.
        if (_jumpingActive && !grounded)
            _leftGroundSinceJump = true;

        // Countdown jump safety timer
        if (_jumpTimer > 0f)
            _jumpTimer -= dt;

        // End jump when:
        // - we've left ground during this jump AND we are grounded again (landing), OR
        // - timeout
        bool landedNow = (!_wasGrounded && grounded);
        if (_jumpingActive)
        {
            if ((_leftGroundSinceJump && grounded) || _jumpTimer <= 0f)
            {
                _jumpingActive = false;
                _leftGroundSinceJump = false;
                _jumpTimer = 0f;
            }
        }

        animator.SetBool(IsJumping, _jumpingActive);

        // --- Land bool (held for landHoldTime) ---
        // Set land window when we detect airborne -> grounded transition.
        if (landedNow)
            _landTimer = landHoldTime;

        if (_landTimer > 0f)
            _landTimer -= dt;

        animator.SetBool(Land, _landTimer > 0f);

        _wasGrounded = grounded;
    }

    public void NotifyJumpStarted()
    {
        _jumpingActive = true;
        _leftGroundSinceJump = false;
        _jumpTimer = jumpMaxTime;
        _landTimer = 0f;
    }
    
    private float ComputePlanarSpeed01Rounded3()
    {
        if (movementData == null) return 0f;
        if (maxSpeed <= 0.0001f) return 0f;

        Vector3 v = movementData.Velocity;

        // Planet-aware up to remove vertical component
        Vector3 planetUp = Vector3.up;
        if (movementData.WorldMiddle != null)
        {
            Vector3 toPlayer = transform.position - movementData.WorldMiddle.position;
            if (toPlayer.sqrMagnitude > 0.0001f)
                planetUp = toPlayer.normalized;
        }

        float planarSpeed = Vector3.ProjectOnPlane(v, planetUp).magnitude;
        float t = Mathf.InverseLerp(0f, maxSpeed, planarSpeed);

        // round to 3 decimals
        t = Mathf.Round(t * 1000f) / 1000f;
        return t;
    }
}