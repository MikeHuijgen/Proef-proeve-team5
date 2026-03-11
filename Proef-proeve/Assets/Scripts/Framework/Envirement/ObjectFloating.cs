using System;
using UnityEngine;

public class ObjectFloating : MonoBehaviour
{
    [SerializeField] private Transform floatingObject;
    [SerializeField] private float floatDuration = 2f;
    [SerializeField] private float floatHeight = 2f;

    private CountdownTimer _floatCountdownTimer;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private bool _movingUp = true;

    private void Start()
    {
        _startPosition = floatingObject.position;

        _targetPosition = _startPosition + floatingObject.up * floatHeight;

        _floatCountdownTimer = new CountdownTimer(floatDuration);
        _floatCountdownTimer.StartTimer();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        _floatCountdownTimer.Tick(deltaTime);

        Vector3 target = _movingUp ? _targetPosition : _startPosition;

        floatingObject.position = Vector3.MoveTowards(
            floatingObject.position,
            target,
            (floatHeight / floatDuration) * deltaTime
        );

        if (!_floatCountdownTimer.IsTimerDone) return;

        _floatCountdownTimer.StopTimer();

        _movingUp = !_movingUp;

        _floatCountdownTimer = new CountdownTimer(floatDuration);
        _floatCountdownTimer.StartTimer();
    }
}