using System;
using UnityEngine;
using UnityEngine.Events;

public class TravelState : BaseState
{
    public UnityEvent OnTravelStarted = new UnityEvent(); 
    public UnityEvent OnTravelEnded = new UnityEvent(); 

    [SerializeField] private GameObject meshRenderer;
    private PlayerGravity _playerGravity;
    private MovementMotor _movementMotor;
    private MovementData _movementData;
    

    private void Start()
    {
        _playerGravity = GetComponent<PlayerGravity>();
        _movementMotor = GetComponent<MovementMotor>();
        _movementData = GetComponent<MovementData>();
        Launch.OnLanded += OnLand;
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        _playerGravity.ResetGravity();
        _playerGravity.enabled = false;
        _movementMotor.enabled = false;
        _movementData.enabled = false;
        meshRenderer.SetActive(false);

        OnTravelStarted?.Invoke();
    }

    public override void StateExit()
    {
        _playerGravity.enabled = true;
        _movementMotor.enabled = true;  
        _movementData.enabled = true;
        meshRenderer.SetActive(true); 
        OnTravelEnded?.Invoke();     
    }

    public override void StateUpdate(float deltaTime)
    {
        
    }

    private void OnLand() => _onStateCompleted();
}
