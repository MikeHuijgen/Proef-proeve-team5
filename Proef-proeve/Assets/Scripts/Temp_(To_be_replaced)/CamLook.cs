using UnityEngine;
using UnityEngine.InputSystem;

public class CamLook : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float sensitivity = 0.15f;

    private InputAction lookAction;

    private float yaw;

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
    }

    private void Update()
    {
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        yaw += lookDelta.x * sensitivity;

        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }
}