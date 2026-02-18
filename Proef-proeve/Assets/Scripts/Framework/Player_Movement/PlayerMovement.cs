using UnityEngine;

public class PlayerMovement : MovementComponent
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private float _horizontalInput;
    private float _verticalInput;

    private PlayerGravity _gravity;

    private void Start()
    {
        _gravity = GetComponent<PlayerGravity>();
    }

    private void Update()
    {
        Vector3 planetNormal =
            (transform.position - MovementData.WorldMiddle.position).normalized;

        CheckInput();

        _gravity.UpdateGravity(-planetNormal);

        MovePlayer(planetNormal);
        AlignToPlanet(planetNormal);
    }

    private void MovePlayer(Vector3 normal)
    {
        Vector3 camForward =
            Vector3.ProjectOnPlane(MovementData.Camera.forward, normal).normalized;

        Vector3 camRight =
            Vector3.ProjectOnPlane(MovementData.Camera.right, normal).normalized;

        Vector3 inputMoveDir =
            camRight * _horizontalInput + camForward * _verticalInput;

        Vector3 horizontalVelocity = Vector3.zero;

        if (inputMoveDir.sqrMagnitude > 0.001f)
        {
            inputMoveDir.Normalize();
            horizontalVelocity = inputMoveDir * _moveSpeed;

            RotateBodyTowardsMovement(inputMoveDir);
        }

        Vector3 verticalVelocity = _gravity.GravityVelocity;
        
        Vector3 totalVelocity = horizontalVelocity + verticalVelocity;

        MovementData.CharacterController.Move(
            totalVelocity * Time.deltaTime
        );
        
        
    }

    private void AlignToPlanet(Vector3 normal)
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

        transform.rotation = targetRotation;
    }

    private void RotateBodyTowardsMovement(Vector3 moveDir)
    {
        Vector3 localMoveDir = transform.InverseTransformDirection(moveDir);
        float targetYaw =
            Mathf.Atan2(localMoveDir.x, localMoveDir.z) * Mathf.Rad2Deg;

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
        _verticalInput   = input.y;
    }
}
