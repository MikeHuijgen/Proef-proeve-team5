using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static EventHandler<string> OnNewActiveState;

    [SerializeField] private IntentToState[] intentDataToStates;
    [SerializeField] private BaseState defaultState;
    private BaseState _currentActiveState;
    private Dictionary<StateIntentData, BaseState> _intentDataToStateDictionary;

    private void Awake()
    {
        PopulateIntentDataToStateDictionary();
        SwitchState(defaultState);
    }

    private void OnEnable() => InputHandler.Instance.OnNewStateIntent += OnNewStateIntent;

    void OnDisable() => InputHandler.Instance.OnNewStateIntent -= OnNewStateIntent;

    private void OnNewStateIntent(object sender, StateIntentData intentData)
    {
        var state = GetStateByIntentData(intentData);
        if (state == null || state == _currentActiveState || !_currentActiveState.CanBeInterrupted) return;

        SwitchState(state);
    }

    private void SwitchState(BaseState newState)
    {
        _currentActiveState?.StateExit();
        _currentActiveState = newState;
        _currentActiveState?.StateEnter(OnStateCompleted);

        OnNewActiveState?.Invoke(this, newState.ToString());
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
}
