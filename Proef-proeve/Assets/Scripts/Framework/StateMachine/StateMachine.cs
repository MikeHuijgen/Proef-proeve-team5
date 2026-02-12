using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private IntentToState[] intentToStates;
    private BaseState _currentState;
    private Dictionary<int, BaseState> _intentToStateDictionary;

    private void OnEnable()
    {
        //Hier moeten we op de inputhandler eventhandler aanmelden
    }

    void OnDisable()
    {
        //Hier moeten we op de inputhandler eventhandler afmelden 
    }

    private void SwitchState(BaseState newState)
    {
        _currentState.StateExit();
        _currentState = newState;
        _currentState.StateEnter();
    }

    private void OnNewIntent()
    {
        
    }
}
