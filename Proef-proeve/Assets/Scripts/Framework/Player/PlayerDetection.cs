using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Ground Detection")]
    public Vector3 groundBoxSize = new Vector3(0.9f, 0.1f, 0.9f);
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Wall Detection")]
    public Vector3 wallBoxSize = new Vector3(0.9f, 1f, 0.1f);
    public float wallCheckDistance = 0.2f;
    public LayerMask wallLayer;

    public bool IsGrounded { get; private set; }
    public bool IsTouchingWall { get; private set; }

    public bool CheckGround()
    {
        Vector3 origin = transform.position;
        Vector3 halfExtents = groundBoxSize / 2f;

        return Physics.BoxCast(
            origin,
            halfExtents,
            Vector3.down,
            out RaycastHit hit,
            transform.rotation,
            groundCheckDistance,
            groundLayer
        );
    }

    public bool CheckWall()
    {
        Vector3 origin = transform.position;
        Vector3 halfExtents = wallBoxSize / 2f;

        return Physics.BoxCast(
            origin,
            halfExtents,
            transform.forward,
            out RaycastHit hit,
            transform.rotation,
            wallCheckDistance,
            wallLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        DrawBoxCast(transform.position, groundBoxSize, Vector3.down, groundCheckDistance);

        Gizmos.color = Color.blue;
        DrawBoxCast(transform.position, wallBoxSize, transform.forward, wallCheckDistance);
    }

    void DrawBoxCast(Vector3 origin, Vector3 size, Vector3 direction, float distance)
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(direction * distance, size);
        Gizmos.matrix = originalMatrix;
    }
}
