using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    private bool _canBeInterrupted;

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
    public virtual void OnStateInterrupted(){}

    public bool CanBeInterrupted => _canBeInterrupted;
}
