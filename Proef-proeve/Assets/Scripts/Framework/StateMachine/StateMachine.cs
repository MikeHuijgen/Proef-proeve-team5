using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StateMachine : MonoBehaviour
{
    public static event Action<string> OnNewActiveState;

    [SerializeField] private IntentToState[] intentDataToStates;
    
    [SerializeField] private BaseState defaultGroundedState;

    [SerializeField] private BaseState defaultAirborneState;

    private BaseState _currentActiveState;
    private Dictionary<StateIntentData, BaseState> _intentDataToStateDictionary;

    private GroundCheck _groundCheck;
    private JumpBuffer _jumpBuffer;

    private void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _jumpBuffer = GetComponent<JumpBuffer>();
    }

    private void Awake()
    {
        PopulateIntentDataToStateDictionary();
        SwitchState(defaultGroundedState);
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnNewStateIntent += OnNewStateIntent;
        _jumpBuffer.OnConfirmJump += OnNewStateIntent;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnNewStateIntent -= OnNewStateIntent;
        _jumpBuffer.OnConfirmJump -= OnNewStateIntent;
    }

    private void OnNewStateIntent(StateIntentData intentData)
    {
        var state = GetStateByIntentData(intentData);
        if (!CheckAllConditions(state)) return;

        SwitchState(state);
    }

    private bool CheckAllConditions(BaseState newState)
    {
        if (newState == null) return false;

        if (!newState.AllowedInAirborne && !_groundCheck.IsGrounded) return false;
        
        if (newState == _currentActiveState || !CheckInterruptPermission(newState)) return false;

        return true;
    }
    
    private void SwitchState(BaseState newState)
    {
        _currentActiveState?.StateExit();
        _currentActiveState = newState;
        _currentActiveState?.StateEnter(OnStateCompleted);

        OnNewActiveState?.Invoke(newState.ToString());
    }

    private void Update() => _currentActiveState?.StateUpdate(Time.deltaTime);


    private BaseState GetDesiredDefaultState()
    {
        if (_groundCheck == null) return defaultGroundedState;

        if (_groundCheck.IsGrounded)
            return defaultGroundedState;

        return defaultAirborneState;
    }

    private BaseState GetStateByIntentData(StateIntentData intentData)
    {
        _intentDataToStateDictionary.TryGetValue(intentData, out var state);
        return state;
    }

    private void OnStateCompleted()
    {
        SwitchState(GetDesiredDefaultState());
    }

    private void PopulateIntentDataToStateDictionary()
    {
        _intentDataToStateDictionary = new Dictionary<StateIntentData, BaseState>();

        foreach (var intentDataToState in intentDataToStates)
        {
            if (_intentDataToStateDictionary.ContainsKey(intentDataToState.stateIntentData)) continue;
            _intentDataToStateDictionary.Add(intentDataToState.stateIntentData, intentDataToState.state);
        }
    }

    private bool CheckInterruptPermission(BaseState newState)
    {
        if (_currentActiveState == null) return false;

        foreach (var interruptPermission in _currentActiveState.GetInterruptPermission)
        {
            if (interruptPermission != newState) continue;
            return true;
        }

        return false;
    }
}