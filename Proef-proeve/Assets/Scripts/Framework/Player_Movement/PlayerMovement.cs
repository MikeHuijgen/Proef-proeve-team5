using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _worldMiddle;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private CharacterController _characterController;
    private float _horizontalInput;
    private float _verticalInput;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 normal = (transform.position - _worldMiddle.position).normalized;

        CheckInput();
        AlignToPlanet(normal);
        MovePlayer(normal);
    }

    private void MovePlayer(Vector3 normal)
    {
        Vector3 camForward = Vector3.ProjectOnPlane(_camera.forward, normal).normalized;
        Vector3 camRight   = Vector3.ProjectOnPlane(_camera.right, normal).normalized;

        Vector3 moveDir = camRight * _horizontalInput + camForward * _verticalInput;

        if (moveDir.sqrMagnitude < 0.001f)
            return;

        moveDir.Normalize();

        _characterController.Move(moveDir * _moveSpeed * Time.deltaTime);
        RotateBodyTowardsMovement(moveDir);
    }

    /// <summary>
    /// For aligning rotation to the planet only
    /// </summary>
    /// <param name="normal"></param>
    private void AlignToPlanet(Vector3 normal)
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

        transform.rotation = targetRotation;
    }

    private void RotateBodyTowardsMovement(Vector3 moveDir)
    {
        Vector3 localMoveDir = transform.InverseTransformDirection(moveDir);

        float targetYaw = Mathf.Atan2(localMoveDir.x, localMoveDir.z) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);

        _playerBody.localRotation = Quaternion.Slerp(
            _playerBody.localRotation,
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
