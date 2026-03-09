using System;
using UnityEngine;

public class TravelState : BaseState
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _meshRenderer.enabled = false;
    }

    public override void StateExit()
    {
        _meshRenderer.enabled = true;
    }

    public override void StateUpdate(float deltaTime)
    {
        
    }
}
