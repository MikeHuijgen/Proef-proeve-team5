using System;
using UnityEngine;

public class StunState : BaseState
{
    public static event Action OnStunStateExit;
    [SerializeField] private float stunDuration;
    private Animator _playerAnimator;
    private MovementData _movementData;
    private CountdownTimer _stunCountdownTimer;

    private void Start()
    {
        _movementData = GetComponent<MovementData>();
        _playerAnimator = GetComponent<PlayerAnimationManager>().animator;
    }

    public override void StateEnter(Action onStateCompleted)
    {
        _playerAnimator.SetBool("IsStunned", true);
        _onStateCompleted = onStateCompleted;
        _movementData.enabled = false;
        _stunCountdownTimer = new CountdownTimer(stunDuration);
        _stunCountdownTimer.StartTimer();
    }

    public override void StateExit()
    {
        _playerAnimator.SetBool("IsStunned", false);
        _movementData.enabled = true;
        CheckpointSystem.Instance.RespawnPlayer();
        OnStunStateExit?.Invoke();
    }

    public override void StateUpdate(float deltaTime)
    {
        _stunCountdownTimer.Tick(deltaTime);

        if (!_stunCountdownTimer.IsTimerDone) return;
        _stunCountdownTimer.StopTimer();
        _onStateCompleted();
    }


}
