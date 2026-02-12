using System;
using UnityEngine;

public class MoveState : BaseState
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody playerRigidbody;
    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        Debug.Log("Entered Move State");
    }

    public override void StateExit()
    {

    }

    public override void StateUpdate(float deltaTime)
    {
        Vector2 moveInput = InputHandler.Instance.GetMoveValue();

        // Convert 2D input to 3D movement
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move the Rigidbody
        playerRigidbody.MovePosition(playerRigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
