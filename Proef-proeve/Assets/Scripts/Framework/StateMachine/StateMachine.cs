using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;

    private void SwitchState(BaseState newState)
    {
        _currentState.StateExit();
        _currentState = newState;
        _currentState.StateEnter();
    }
}
