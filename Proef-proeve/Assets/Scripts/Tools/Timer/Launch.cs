using System;
using Unity.VisualScripting;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public static event Action<StateIntentData> OnLaunch;
    public static event Action<Transform> OnPlanetChange;
    public static event Action OnLanded;

    [SerializeField] private StateIntentData travelIntent;
    [SerializeField] private Transform endLocation;
    [SerializeField]private Transform nextPlanet;
    [SerializeField] private float travelDuration = 3f;

    private Quaternion _targetRotation;
    private bool _rotateToTarget;


    private Transform _playerTransform;

    private BoxCollider _boxCollider;
    private Vector3 _startPosition;
    private bool _isTravel;
    private float _travelTime;


    private float _distanceThreshold = .05f;

    private void Start()
    { 
        _startPosition = transform.position;
        _boxCollider = GetComponent<BoxCollider>();
        
    }

    private void Update()
    {
        if (!_isTravel) return;
        Traveling();
    }

    private void Traveling()
    {
        if (_travelTime >= travelDuration) return;

        _travelTime += Time.deltaTime;
        var currentTravelTime = _travelTime / travelDuration;

        transform.position = Vector3.Lerp(_startPosition, endLocation.position, currentTravelTime);

        if (Vector3.Distance(transform.position, endLocation.position) > _distanceThreshold) return;

        _isTravel = false;
        _playerTransform.parent = null;
        OnLanded?.Invoke();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        _isTravel =  true;  
        _boxCollider.enabled = false;

        _playerTransform = collision.gameObject.transform;
        _playerTransform.parent = transform;

        OnLaunch?.Invoke(travelIntent);
        OnPlanetChange?.Invoke(nextPlanet);
    }




        // if player makes collision with somting, travel is true than.
        // if (collision.CompareTag("Player"))
        // {
        //     OnLaunch?.Invoke(travelIntent);
        //     _gravity.ResetGravity();
        //     _gravity.enabled = false;
        //     _movementData.WorldMiddle = _nextPlanet;
        //     _travelTime = Time.time;
        //     _isTravel =  true;
        //     _player.transform.SetParent(transform);
        //     _capsuleCollider.enabled = false;
        //     _playerMovement.enabled = false;
        // }
}
