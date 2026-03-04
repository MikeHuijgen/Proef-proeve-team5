using UnityEngine;
using UnityEngine.InputSystem;

public class CamLook : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float sensitivity = 0.15f;

    private InputAction lookAction;

    private float yaw;
    private float xaw;

    private void Awake()
    {
        lookAction = playerInput.actions["Look"];
    }

    private void OnEnable()
    {
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        xaw = transform.eulerAngles.x;
    }

    private void Update()
    {
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        xaw = Mathf.Clamp(xaw, -45f, 45f);

        yaw += lookDelta.x * sensitivity;

        xaw += lookDelta.y * sensitivity;

        transform.localRotation = Quaternion.Euler(xaw, yaw, 0f);
    }
}