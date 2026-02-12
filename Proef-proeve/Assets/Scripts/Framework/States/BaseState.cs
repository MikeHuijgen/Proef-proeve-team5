using System;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    private bool _canBeInterrupted;
    protected Action OnStateCompleted;

    public abstract void StateEnter(Action OnStateCompleted);
    public abstract void StateUpdate();
    public abstract void StateExit();
    
    public bool CanBeInterrupted => _canBeInterrupted;
}
