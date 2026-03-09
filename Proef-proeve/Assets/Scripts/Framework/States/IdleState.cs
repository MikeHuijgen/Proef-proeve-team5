using System;
using UnityEngine;

public class IdleState : BaseState
{
    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
    }

    public override void StateExit()
    {

    }

    public override void StateUpdate(float deltaTime)
    {

    }
}
