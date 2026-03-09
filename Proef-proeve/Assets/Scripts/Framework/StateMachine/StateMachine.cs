using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static event Action<string> OnNewActiveState;

    [SerializeField] private IntentToState[] intentDataToStates;
    [SerializeField] private BaseState defaultState;
    private BaseState _currentActiveState;
    private Dictionary<StateIntentData, BaseState> _intentDataToStateDictionary;
    private PlayerStunRequestHandler _playerStunRequestHandler;

    private void Awake()
    {
        _playerStunRequestHandler = GetComponent<PlayerStunRequestHandler>();
        PopulateIntentDataToStateDictionary();
        SwitchState(defaultState);
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnNewStateIntent += OnNewStateIntent;
        _playerStunRequestHandler.OnStunRequest += OnNewStateIntent;
    }

    void OnDisable()
    {
        InputHandler.Instance.OnNewStateIntent -= OnNewStateIntent;
        _playerStunRequestHandler.OnStunRequest -= OnNewStateIntent;
    }

    public void OnNewStateIntent(StateIntentData intentData)
    {
        var state = GetStateByIntentData(intentData);
        if (!CheckAllConditions(state)) return;

        SwitchState(state);
    }

    private bool CheckAllConditions(BaseState newState)
    {
        if (newState == null 
        || newState == _currentActiveState 
        || !CheckInterruptPermission(newState)) return false;
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

    private BaseState GetStateByIntentData(StateIntentData intentData)
    {
        _intentDataToStateDictionary.TryGetValue(intentData, out var state);
        return state;
    }

    private void OnStateCompleted() => SwitchState(defaultState);

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
