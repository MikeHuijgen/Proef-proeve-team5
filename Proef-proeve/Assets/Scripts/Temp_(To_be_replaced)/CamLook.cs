using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class CamLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;

    private bool _allowedToMoveCamera;
    private float _yaw;
    private float _xaw;

    private void Start()
    {
        _yaw = transform.eulerAngles.y;
        _xaw = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnFingerTouchInputDown += OnFingerTouchInputDown;
        InputHandler.Instance.OnFingerTouchInputUp += OnFingerTouchInputUp;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFingerTouchInputDown -= OnFingerTouchInputDown;
        InputHandler.Instance.OnFingerTouchInputUp -= OnFingerTouchInputUp;        
    }

    private void OnFingerTouchInputDown() => _allowedToMoveCamera = true;
    private void OnFingerTouchInputUp() => _allowedToMoveCamera = false;

    private void Update() => RotateCamera();
    

    private void RotateCamera()
    {
        if (!_allowedToMoveCamera) return;

        var lookDelta = InputHandler.Instance.GetCameraValue();
        //print(InputHandler.Instance.GetCameraValue());

        _xaw = Mathf.Clamp(_xaw, -45f, 45f);

        _yaw += lookDelta.x * -sensitivity;
        _xaw += lookDelta.y * sensitivity;

        transform.localRotation = Quaternion.Euler(_xaw, _yaw, 0f);       
    }
}