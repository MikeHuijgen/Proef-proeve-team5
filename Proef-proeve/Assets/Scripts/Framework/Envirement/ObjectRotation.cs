using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 50f;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}