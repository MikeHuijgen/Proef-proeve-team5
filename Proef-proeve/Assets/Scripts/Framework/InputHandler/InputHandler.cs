using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public EventHandler<IntentData> OnNewIntent;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputToIntent[] inputToIntents;

    private Dictionary<Guid, IntentData> _idToIntent;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        PopulateDictionaryWithIdAndIntent();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMoveInputDetected;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMoveInputDetected;      
    }

    private void OnMoveInputDetected(InputAction.CallbackContext context) => OnNewIntent?.Invoke(this, GetIntentByInput(context));

    private void PopulateDictionaryWithIdAndIntent()
    {
        _idToIntent = new Dictionary<Guid, IntentData>();

        foreach (var inputToIntent in inputToIntents)
        {
            var reference = inputToIntent.inputActionReference;
            if (reference == null && reference.action == null) continue;

            _idToIntent[reference.action.id] = inputToIntent.intentSO;
        }
    }

    private IntentData GetIntentByInput(InputAction.CallbackContext context)
    {
        var action = context.action;
        if (action == null) return null;
        _idToIntent.TryGetValue(action.id, out var intentData);
        return intentData;
    }

    public Vector2 GetMoveValue() => playerInput.actions["Move"].ReadValue<Vector2>();
}
