using UnityEngine;

[RequireComponent(typeof(MovementData))]
[RequireComponent(typeof(PlayerGravity))]
[RequireComponent(typeof(GroundCheck))]
public class MovementMotor : MovementComponent
{
    private PlayerGravity _gravity;
    private GroundCheck _groundCheck;
    private CharacterController _characterController;

    private void Awake()
    {
        _gravity = GetComponent<PlayerGravity>();
        _groundCheck = GetComponent<GroundCheck>();

        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 controllerCenterWorld = _characterController.transform.TransformPoint(_characterController.center);
        Vector3 planetUp = (controllerCenterWorld - MovementData.WorldMiddle.position).normalized;
        Vector3 gravityDown = -planetUp;

        bool grounded = _groundCheck != null && _groundCheck.IsGrounded;

        Vector3 movePlaneNormal = planetUp;
        if (grounded)
        {
            Vector3 n = _groundCheck.GroundHit.normal;
            if (n.sqrMagnitude > 0.0001f) movePlaneNormal = n.normalized;
        }

        if (MovementData.JumpRequested && grounded)
        {
            float jumpSpeed = Mathf.Sqrt(2f * _gravity.GravityStrength * MovementData.JumpHeight);
            _gravity.Jump(jumpSpeed);
            MovementData.ConsumeJumpRequest();
        }

        _gravity.UpdateGravity(gravityDown, grounded);

        Vector3 horizontalVelocity = ComputeHorizontalVelocity(movePlaneNormal, MovementData.MoveInput, MovementData.MoveSpeed);

        _characterController.Move(horizontalVelocity * Time.deltaTime);
        _characterController.Move(_gravity.GravityVelocity * Time.deltaTime);

        AlignToPlanet(planetUp);

        if (horizontalVelocity.sqrMagnitude > 0.0001f)
            RotateBodyTowardsMovement(horizontalVelocity.normalized, MovementData.RotationSpeed);
    }

    private Vector3 ComputeHorizontalVelocity(Vector3 planeNormal, Vector2 moveInput, float speed)
    {
        Vector3 camForward = Vector3.ProjectOnPlane(MovementData.Camera.forward, planeNormal).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(MovementData.Camera.right, planeNormal).normalized;

        Vector3 desired = camRight * moveInput.x + camForward * moveInput.y;
        if (desired.sqrMagnitude <= 0.001f) return Vector3.zero;

        desired.Normalize();
        desired = Vector3.ProjectOnPlane(desired, planeNormal).normalized;

        return desired * speed;
    }

    private void AlignToPlanet(Vector3 planetUp)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, planetUp) * transform.rotation;
        transform.rotation = targetRotation;
    }

    private void RotateBodyTowardsMovement(Vector3 moveDir, float rotationSpeed)
    {
        Vector3 localMoveDir = transform.InverseTransformDirection(moveDir);
        float targetYaw = Mathf.Atan2(localMoveDir.x, localMoveDir.z) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);

        MovementData.PlayerBody.localRotation = Quaternion.Slerp(
            MovementData.PlayerBody.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}