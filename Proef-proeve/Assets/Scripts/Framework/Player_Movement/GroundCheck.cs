using UnityEngine;

public class GroundCheck : MovementComponent
{
    [SerializeField] private float _groundCheckDistance = 1.2f;
    [SerializeField] private LayerMask _groundLayer;
    
    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        DoGroundCheck();
    }

    /*private void DoGroundCheck()
    {
        Vector3 origin = transform.position;
        float radius = MovementData.CharacterController.radius;
        Vector3 direction = (MovementData.WorldMiddle.position - transform.position).normalized;

        IsGrounded = Physics.SphereCast(
            origin,
            radius,
            direction,
            _groundCheckDistance,
            _groundLayer
        );

        Debug.DrawRay(origin, direction * _groundCheckDistance,
            IsGrounded ? Color.green : Color.red);
    }*/
    
    private void DoGroundCheck()
    {
        float radius = MovementData.CharacterController.radius;
        float skin = 0.05f;

        Vector3 gravityDown = (MovementData.WorldMiddle.position - transform.position).normalized;
        Vector3 gravityUp   = -gravityDown;
        
        Vector3 origin = transform.position + gravityUp * (radius + skin);

        IsGrounded = Physics.SphereCast(
            origin,
            radius,
            gravityDown,
            out RaycastHit hit,
            _groundCheckDistance,
            _groundLayer,
            QueryTriggerInteraction.Ignore
        );

        Debug.DrawRay(
            origin,
            gravityDown * _groundCheckDistance,
            IsGrounded ? Color.green : Color.red
        );
    }

}
