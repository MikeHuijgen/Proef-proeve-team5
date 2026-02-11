using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _worldMiddle;

    [SerializeField] private float _moveSpeed;

    private CharacterController _characterController;
    
    private float _horizontalInput;
    private float _verticalInput;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckInput();
        //CheckGroundNormal();
        MovePlayer();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        
    }
    
    private void MovePlayer()
    {
        Vector3 normal = (transform.position - _worldMiddle.position).normalized;
        
        
        Vector3 moveDir = Vector3.ProjectOnPlane(new Vector3(_horizontalInput, 0, _verticalInput), normal).normalized;

        Vector3 finalMove = moveDir * _moveSpeed;
        
        _characterController.Move(finalMove * Time.deltaTime);
    }
    
    /*private void CheckGroundNormal()
    {
        
    }*/
    
    private void CheckInput()
    {
        _horizontalInput = InputHandler.Instance.GetMoveValue().x;
        _verticalInput = InputHandler.Instance.GetMoveValue().y;
    }
}
