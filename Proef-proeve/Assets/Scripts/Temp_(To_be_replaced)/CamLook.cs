using UnityEngine;
using UnityEngine.InputSystem;

public class CamLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;

    private InputAction lookAction;

    private float yaw;
    private float xaw;


    private void Start()
    {
        yaw = transform.eulerAngles.y;
        xaw = transform.eulerAngles.x;
    }

    private void Update()
    {

        Vector2 lookDelta = InputHandler.Instance.GetCameraValue();

        xaw = Mathf.Clamp(xaw, -45f, 45f);

        yaw += lookDelta.x *- sensitivity;

        xaw += lookDelta.y * sensitivity;

        transform.localRotation = Quaternion.Euler(xaw, yaw, 0f);
    }
}