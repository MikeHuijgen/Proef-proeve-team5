using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CamLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;

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

        // MOBILE: ignore input over UI
        if (Touchscreen.current != null) // Only on mobile
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.isPressed && EventSystem.current.IsPointerOverGameObject((int)touch.touchId.ReadValue()))
                {
                    return; // ignore this frame if over UI
                }
            }
        }

        xaw = Mathf.Clamp(xaw, -45f, 45f);

        yaw += lookDelta.x * -sensitivity;
        xaw += lookDelta.y * sensitivity;

        transform.localRotation = Quaternion.Euler(xaw, yaw, 0f);
    }
}