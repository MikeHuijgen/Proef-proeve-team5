using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public EventHandler<IntentData> OnMoveInput;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputToIntent[] inputToIntents;

    private Dictionary<Guid, IntentData> _idToIntent;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        PopulateDictionary();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMoveInputDetected;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMoveInputDetected;      
    }

    private void OnMoveInputDetected(InputAction.CallbackContext context) => OnMoveInput?.Invoke(this, GetIntentWithInput(context));

    private void PopulateDictionary()
    {
        _idToIntent = new Dictionary<Guid, IntentData>();

        foreach (var inputToIntent in inputToIntents)
        {
            var reference = inputToIntent.inputActionReference;
            if (reference == null && reference.action == null) continue;

            _idToIntent[reference.action.id] = inputToIntent.intentSO;
        }
    }

    private IntentData GetIntentWithInput(InputAction.CallbackContext context)
    {
        var action = context.action;
        if (action == null) return null;
        _idToIntent.TryGetValue(action.id, out var stateIntent);
        return stateIntent;
    }

    public Vector2 GetMoveValue() => playerInput.actions["Move"].ReadValue<Vector2>();
}
