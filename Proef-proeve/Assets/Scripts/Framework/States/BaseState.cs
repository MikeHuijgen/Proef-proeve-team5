using System;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] private bool canBeInterrupted = true;
    protected Action _onStateCompleted;

    public abstract void StateEnter(Action onStateCompleted);
    public abstract void StateUpdate(float deltaTime);
    public abstract void StateExit();

    public bool CanBeInterrupted => canBeInterrupted;
}
