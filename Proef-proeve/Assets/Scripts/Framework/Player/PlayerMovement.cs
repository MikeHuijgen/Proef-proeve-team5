using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerDetection
{
    [Header("Movement Settings")]
    [SerializeField] private float MoveSpeed = 5f;
    private Rigidbody _rb;

        private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Optional: freeze rotation if you don't want physics rotation
        _rb.freezeRotation = true;
    }

    void Update()
    {
        Debug.Log(InputHandler.Instance.GetMoveValue());
    }
}
