using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public event Action<StateIntentData> OnNewStateIntent;
    public event Action<int> OnCameraFingerTouchDown;
    public event Action OnCameraFingerTouchUp;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputToIntent[] inputToIntents;

    private Dictionary<Guid, StateIntentData> _idToIntent;

    private int _cameraFingerId;

    private List<int> _uiFingerIds;
    private int _defaultFingerIdValue = -1;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        PopulateDictionaryWithIdAndIntent();

        _cameraFingerId = _defaultFingerIdValue;
        _uiFingerIds = new List<int>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        playerInput.actions["Move"].performed += OnIntentInputDetected;

        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        playerInput.actions["Move"].performed -= OnIntentInputDetected; 

        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger targetFinger)
    {
        var targetFingerId = targetFinger.index;

        if (IsTouchOverUI(targetFinger))
        {
            AddUiFingerId(targetFingerId);
            return;
        }

        if (_uiFingerIds.Contains(targetFingerId)) return;

        _cameraFingerId = targetFingerId;
        OnCameraFingerTouchDown?.Invoke(targetFingerId);
    }

    private void OnFingerUp(Finger targetFinger)
    {
        var targetFingerId = targetFinger.index;

        if (targetFingerId != _cameraFingerId) 
        {
            RemoveUiFingerId(targetFingerId);
            return;
        }

        _cameraFingerId = _defaultFingerIdValue;   
        OnCameraFingerTouchUp?.Invoke();   
    }

    private bool IsTouchOverUI(Finger finger)
    {
        var eventData = new PointerEventData(EventSystem.current) {position = finger.screenPosition};

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }


    private void OnIntentInputDetected(InputAction.CallbackContext context)
    {
        var stateIntentData = GetIntentDataByInputId(context);
        if(stateIntentData == null) return;
        OnNewStateIntent?.Invoke( stateIntentData);
    }

    private void PopulateDictionaryWithIdAndIntent()
    {
        _idToIntent = new Dictionary<Guid, StateIntentData>();

        foreach (var inputToIntent in inputToIntents)
        {
            var reference = inputToIntent.inputActionReference;
            if (reference == null && reference.action == null) continue;

            _idToIntent[reference.action.id] = inputToIntent.intentSO;
        }
    }

    private StateIntentData GetIntentDataByInputId(InputAction.CallbackContext context)
    {
        var action = context.action;
        if (action == null) return null;
        _idToIntent.TryGetValue(action.id, out var intentData);
        return intentData;
    }

    private void AddUiFingerId(int fingerId)
    {
        if (_uiFingerIds.Contains(fingerId)) return;

        _uiFingerIds.Add(fingerId);
    }

    private void RemoveUiFingerId(int fingerId)
    {
        if (!_uiFingerIds.Contains(fingerId)) return;

        _uiFingerIds.Remove(fingerId);
    }

    public Vector2 GetMoveValue() => playerInput.actions["Move"].ReadValue<Vector2>();

    public Vector2 GetCameraValue()
    {
        if (_cameraFingerId == _defaultFingerIdValue)
            return Vector2.zero;

        var touch = Touch.activeTouches.FirstOrDefault(f => f.finger.index == _cameraFingerId);

        if (touch.finger == null)
            return Vector2.zero;

        return touch.delta;
    }
}
