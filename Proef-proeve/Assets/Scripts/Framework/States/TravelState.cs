using System;
using UnityEngine;

public class TravelState : BaseState
{
    [SerializeField] private GameObject meshRenderer;
    private PlayerGravity _playerGravity;
    private MovementMotor _movementMotor;

    private void Start()
    {
        _playerGravity = GetComponent<PlayerGravity>();
        _movementMotor = GetComponent<MovementMotor>();
        Launch.OnLanded += OnLand;
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _playerGravity.ResetGravity();
        _playerGravity.enabled = false;
        _movementMotor.enabled = false;
        //meshRenderer.SetActive(false);
    }

    public override void StateExit()
    {
        _playerGravity.enabled = true;
        _movementMotor.enabled = true;  
        meshRenderer.SetActive(true);      
    }

    public override void StateUpdate(float deltaTime)
    {
        
    }

    private void OnLand() => _onStateCompleted();
}
