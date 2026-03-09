using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public event Action<StateIntentData> OnNewStateIntent;
    public event Action OnFingerTouchInputDown;
    public event Action OnFingerTouchInputUp;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputToIntent[] inputToIntents;

    private Dictionary<Guid, StateIntentData> _idToIntent;

    private int _cameraFingerId;

    private List<int> _uiFingerIds;
    private int _defaultFingerIdValue = -1;
    private Vector2 _cameraLookValue;

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

        Touch.Touch.onFingerDown += OnFingerDown;
        Touch.Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        playerInput.actions["Move"].performed -= OnIntentInputDetected; 

        Touch.Touch.onFingerDown -= OnFingerDown;
        Touch.Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger targetFinger)
    {
        var targetFingerId = targetFinger.index;

        if (IsTouchOverUI(targetFinger))
        {
            _uiFingerIds.Add(targetFingerId);
            return;
        }

        _cameraFingerId = targetFingerId;
        OnFingerTouchInputDown?.Invoke();
    }

    private void OnFingerUp(Finger targetFinger)
    {
        var targetFingerId = targetFinger.index;

        if (targetFingerId != _cameraFingerId) 
        {
            _uiFingerIds.Remove(targetFingerId);
            return;
        }

        _cameraFingerId = _defaultFingerIdValue;
        OnFingerTouchInputUp?.Invoke();       
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

    private void RemoveUiFingerId(int fingerId)
    {
        if (!_uiFingerIds.Contains(fingerId)) return;


    }

    public Vector2 GetMoveValue() => playerInput.actions["Move"].ReadValue<Vector2>();

    public Vector2 GetCameraValue() => _cameraLookValue;
}
