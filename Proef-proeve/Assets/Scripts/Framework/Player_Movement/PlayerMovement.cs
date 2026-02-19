using UnityEngine;

public class PlayerMovement : MovementComponent
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private float _horizontalInput;
    private float _verticalInput;

    private PlayerGravity _gravity;
    private GroundCheck _groundCheck;

    private void Start()
    {
        _gravity = GetComponent<PlayerGravity>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void Update()
    {
        CheckInput();

        var cc = MovementData.CharacterController;

        Vector3 controllerCenterWorld = GetControllerCenterWorld(cc);
        Vector3 planetUp = (controllerCenterWorld - MovementData.WorldMiddle.position).normalized;
        Vector3 gravityDown = -planetUp;

        bool grounded = _groundCheck != null && _groundCheck.IsGrounded;

        Vector3 movePlaneNormal = planetUp;
        if (grounded)
        {
            Vector3 n = _groundCheck.GroundHit.normal;
            if (n.sqrMagnitude > 0.0001f) movePlaneNormal = n.normalized;
        }

        _gravity.UpdateGravity(gravityDown, grounded);

        Vector3 horizontalVelocity = ComputeHorizontalVelocity(movePlaneNormal);

        cc.Move(horizontalVelocity * Time.deltaTime);
        cc.Move(_gravity.GravityVelocity * Time.deltaTime);

        AlignToPlanet(planetUp);
    }

    private Vector3 ComputeHorizontalVelocity(Vector3 movePlaneNormal)
    {
        Vector3 camForward =
            Vector3.ProjectOnPlane(MovementData.Camera.forward, movePlaneNormal).normalized;

        Vector3 camRight =
            Vector3.ProjectOnPlane(MovementData.Camera.right, movePlaneNormal).normalized;

        Vector3 inputMoveDir = camRight * _horizontalInput + camForward * _verticalInput;

        if (inputMoveDir.sqrMagnitude <= 0.001f)
            return Vector3.zero;

        inputMoveDir.Normalize();
        inputMoveDir = Vector3.ProjectOnPlane(inputMoveDir, movePlaneNormal).normalized;

        RotateBodyTowardsMovement(inputMoveDir);

        return inputMoveDir * _moveSpeed;
    }

    private void AlignToPlanet(Vector3 planetUp)
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, planetUp) * transform.rotation;

        transform.rotation = targetRotation;
    }

    private void RotateBodyTowardsMovement(Vector3 moveDir)
    {
        Vector3 localMoveDir = transform.InverseTransformDirection(moveDir);
        float targetYaw = Mathf.Atan2(localMoveDir.x, localMoveDir.z) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);

        MovementData.PlayerBody.localRotation = Quaternion.Slerp(
            MovementData.PlayerBody.localRotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    private void CheckInput()
    {
        Vector2 input = InputHandler.Instance.GetMoveValue();
        _horizontalInput = input.x;
        _verticalInput = input.y;
    }

    private static Vector3 GetControllerCenterWorld(CharacterController cc)
    {
        return cc.transform.TransformPoint(cc.center);
    }
}