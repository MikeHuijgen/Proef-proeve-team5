using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 50f; // Rotation speed (degrees per second)

    private void Update()
    {
        // Rotate the object around its Y-axis at the specified speed
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}