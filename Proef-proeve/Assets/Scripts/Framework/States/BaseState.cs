using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] private List<BaseState> stateInterruptPermission = new List<BaseState>();
    [SerializeField] private bool allowedInAirborne = true;

    protected Action _onStateCompleted;

    public abstract void StateEnter(Action onStateCompleted);
    public abstract void StateUpdate(float deltaTime);
    public abstract void StateExit();

    public List<BaseState> GetInterruptPermission => stateInterruptPermission;

    public bool AllowedInAirborne => allowedInAirborne;
}