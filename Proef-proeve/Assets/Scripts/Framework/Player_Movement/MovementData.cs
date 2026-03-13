using System;
using UnityEngine;

public class MovementData : MonoBehaviour
{
    public CharacterController CharacterController;
    public Transform WorldMiddle;
    public Transform Camera;
    public Transform PlayerBody;

    public Vector3 Velocity { get; private set; }

    [Header("Motor Input")]
    public Vector2 MoveInput;
    public float MoveSpeed = 5f;
    public float RotationSpeed = 10;
    public bool JumpRequested;
    public float JumpHeight = 6f;
    
    public float CoyoteTime = 0.12f;
    public float JumpBufferTime = 0.12f;

    private Vector3 _lastPosition;

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        _lastPosition = transform.position;
    }
    private void OnEnable() => Launch.OnPlanetChange += OnPlanetChange;
    private void OnDisable() => Launch.OnPlanetChange -= OnPlanetChange;

    private void OnPlanetChange(Transform newPlanet)
    {
        print(newPlanet);
        WorldMiddle = newPlanet;
    }


    void Update()
    {
        Vector3 currentPosition = transform.position;
        Velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = currentPosition;
    }

    public void ConsumeJumpRequest()
    {
        JumpRequested = false;
    }
}