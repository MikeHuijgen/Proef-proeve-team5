using System;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    private bool _canBeInterrupted;
    protected Action _onStateCompleted;

    public abstract void StateEnter(Action onStateCompleted);
    public abstract void StateUpdate(float deltaTime);
    public abstract void StateExit();

    public bool CanBeInterrupted => _canBeInterrupted;
}
