using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] private List<BaseState> stateInterruptPromission = new List<BaseState>();
    protected Action _onStateCompleted;

    public abstract void StateEnter(Action onStateCompleted);
    public abstract void StateUpdate(float deltaTime);
    public abstract void StateExit();

    public List<BaseState> GetInterruptPromission => stateInterruptPromission;
}
