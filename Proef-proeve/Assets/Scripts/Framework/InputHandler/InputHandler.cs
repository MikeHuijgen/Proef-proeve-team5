using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public event Action<StateIntentData> OnNewStateIntent;

    public event Action OnNewJumpInput;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputToIntent[] inputToIntents;

    private Dictionary<Guid, StateIntentData> _idToIntent;
    
    private InputAction.CallbackContext _lastMoveContext;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        PopulateDictionaryWithIdAndIntent();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnIntentInputDetected;
        playerInput.actions["Jump"].performed += OnJumpInputDetected;
        playerInput.actions["Move"].started += StoreMoveActionCallback;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnIntentInputDetected;      
        playerInput.actions["Jump"].performed -= OnJumpInputDetected;  
        playerInput.actions["Move"].started -= StoreMoveActionCallback;
    }

    private void Update()
    {
        var moveAction = playerInput.actions["Move"];

        if (moveAction.IsPressed())
        {
            OnIntentInputDetected(_lastMoveContext);
        }
    }

    private void OnIntentInputDetected(InputAction.CallbackContext context)
    {
        var stateIntentData = GetIntentDataByInputId(context);
        if(stateIntentData == null) return;
        OnNewStateIntent?.Invoke( stateIntentData);
    }
    
    private void OnJumpInputDetected(InputAction.CallbackContext context) => OnNewJumpInput?.Invoke();
    

    private void PopulateDictionaryWithIdAndIntent()
    {
        _idToIntent = new Dictionary<Guid, StateIntentData>();

        foreach (var inputToIntent in inputToIntents)
        {
            var reference = inputToIntent.inputActionReference;
            if (reference == null || reference.action == null) continue;

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

    private void StoreMoveActionCallback(InputAction.CallbackContext context)
    {
        if ( _lastMoveContext.action != null) return;
        _lastMoveContext = context;
    }

    public Vector2 GetMoveValue() => playerInput.actions["Move"].ReadValue<Vector2>();
}
