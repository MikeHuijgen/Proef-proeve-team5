using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class CamLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;

    private float _yaw;
    private float _xaw;

    private int _currentCameraFingerId;
    private bool _isAllowedToRotateCamera;
    private bool _isUsingRightFingerId;
    private Vector2 _lookDelta;

    private void Start()
    {
        _yaw = transform.eulerAngles.y;
        _xaw = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnCameraFingerTouchDown += OnCameraFingerTouchDown;
        InputHandler.Instance.OnCameraFingerTouchUp += OnCameraFingerTouchUp;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnCameraFingerTouchDown -= OnCameraFingerTouchDown;
        InputHandler.Instance.OnCameraFingerTouchUp -= OnCameraFingerTouchUp;
    }

    private void OnCameraFingerTouchDown(int fingerId)
    {
        _currentCameraFingerId = fingerId;
        _isAllowedToRotateCamera = true;
    }

    private void OnCameraFingerTouchUp()
    {
        _currentCameraFingerId = 0;
        _isAllowedToRotateCamera = false;  
        _isUsingRightFingerId = false;      
    }

    void Update() => RotateCamera();
    
    private void RotateCamera()
    {
        if (!_isAllowedToRotateCamera) return;

        _lookDelta = InputHandler.Instance.GetCameraValue();
        if (_lookDelta == Vector2.zero) return;

        _yaw += _lookDelta.x * sensitivity;
        _xaw += _lookDelta.y * -sensitivity;

        _xaw = Mathf.Clamp(_xaw, -45f, 45f);

        transform.localRotation = Quaternion.Euler(_xaw, _yaw, 0f);
    }
}