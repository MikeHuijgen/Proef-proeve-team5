using UnityEngine;

public class GroundCheck : MovementComponent
{
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private LayerMask _groundLayer;

    [Tooltip("How much smaller than CharacterController.radius the ground check sphere should be.")]
    [SerializeField] private float _radiusInset = 0.01f;

    public bool IsGrounded { get; set; }
    public RaycastHit GroundHit { get; private set; }

    private void Update()
    {
        DoGroundCheck();
    }

    private void DoGroundCheck()
    {
        CharacterController cc = MovementData.CharacterController;

        Vector3 gravityDown = (MovementData.WorldMiddle.position - GetControllerCenterWorld(cc)).normalized;

        float castRadius = Mathf.Max(0.001f, cc.radius - _radiusInset);

        float halfHeight = Mathf.Max(cc.height * 0.5f, cc.radius);
        float bottomHemisphereCenterOffset = halfHeight - cc.radius;

        Vector3 controllerCenterWorld = GetControllerCenterWorld(cc);

        float castDistance = bottomHemisphereCenterOffset + _groundCheckDistance;

        IsGrounded = Physics.SphereCast(
            controllerCenterWorld,
            castRadius,
            gravityDown,
            out RaycastHit hit,
            castDistance,
            _groundLayer,
            QueryTriggerInteraction.Ignore
        );

        GroundHit = hit;

        Debug.DrawRay(
            controllerCenterWorld,
            gravityDown * castDistance,
            IsGrounded ? Color.green : Color.red
        );
    }

    private static Vector3 GetControllerCenterWorld(CharacterController cc)
    {
        return cc.transform.TransformPoint(cc.center);
    }
}