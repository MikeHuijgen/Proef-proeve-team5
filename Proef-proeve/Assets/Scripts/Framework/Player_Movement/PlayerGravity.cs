using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float _gravityStrength = 20f;

    public Vector3 GravityVelocity { get; private set; }

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void UpdateGravity(Vector3 gravityDirection)
    {
        if (_controller.isGrounded)
        {
            GravityVelocity = gravityDirection * 2f;
        }
        else
        {
            GravityVelocity += gravityDirection * _gravityStrength * Time.deltaTime;
        }
    }

    public void ResetGravity()
    {
        GravityVelocity = Vector3.zero;
    }
}