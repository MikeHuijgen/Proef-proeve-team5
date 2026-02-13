using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float _groundCheckDistance = 1.2f;
    [SerializeField] private LayerMask _groundLayer;
    
    public bool IsGrounded { get; private set; }

    private MovementData _movementData;

    private void Awake()
    {
        _movementData = GetComponent<MovementData>();
    }

    private void FixedUpdate()
    {
        DoGroundCheck();
        ApplyGravity();
    }

    private void DoGroundCheck()
    {
        Vector3 origin = transform.position;
        Vector3 direction = (_movementData.WorldMiddle.position - transform.position).normalized;

        IsGrounded = Physics.Raycast(
            origin,
            direction,
            _groundCheckDistance,
            _groundLayer
        );

        Debug.DrawRay(origin, direction * _groundCheckDistance,
            IsGrounded ? Color.green : Color.red);
    }

    private void ApplyGravity()
    {
        
    }

}