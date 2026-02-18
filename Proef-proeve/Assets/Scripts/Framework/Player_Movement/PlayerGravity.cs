using UnityEngine;

public class PlayerGravity : MovementComponent
{
    [SerializeField] private float gravityStrength = 20f;
    [SerializeField] private float groundedGravity = 5f;

    public Vector3 GravityVelocity { get; private set; }

    private GroundCheck _groundCheck;

    private void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void Update()
    {
        var gravityDir = -transform.up;

        if (_groundCheck != null && _groundCheck.IsGrounded)
        {
            GravityVelocity = gravityDir * groundedGravity;
        }
        else
        {
            GravityVelocity += gravityDir * (gravityStrength * Time.deltaTime);
        }
    }
}