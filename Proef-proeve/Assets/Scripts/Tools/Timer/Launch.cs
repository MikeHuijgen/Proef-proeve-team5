using System;
using Unity.VisualScripting;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public static event Action<StateIntentData> OnLaunch;
    public static event Action OnLanded;

    [SerializeField] private StateIntentData travelIntent;

    [SerializeField] private Transform _startLocation;
    [SerializeField] private Transform _endLocation;
    [SerializeField]private Transform _nextPlanet;

    [SerializeField] private GameObject _player;

    [SerializeField] private float _landRotation;
    [SerializeField] private float _speed;
    [SerializeField] private float _travelTime;

    private PlayerGravity _gravity;
    private MovementData _movementData;
    private CapsuleCollider _capsuleCollider;
    private PlayerMovement _playerMovement;


    private float _planetDistance;
    private float _journey;

    private bool _isTravel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    { 
        _movementData = _player.GetComponent<MovementData>();
        _gravity = _player.GetComponent<PlayerGravity>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        _planetDistance = Vector3.Distance(_startLocation.position, _endLocation.position);
    }

    // Update is called once per frame
    private void Update()
    {
        
    // Check if travel is true, and if it is Gameobject start traveling.
        if (_isTravel)
        {
            _journey += Time.deltaTime * _speed / _planetDistance;
            Traveling();
        }
    }

    private void Traveling()
    {
        // check the distandce between planets and devide it with the time it will take to get there.
        transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(_landRotation, 0, 0), _journey);
        transform.position = Vector3.Lerp(_startLocation.position, _endLocation.position, _journey);
        // reset the journey if it is done.
        if (_journey >= 1f)
        {
            _playerMovement.enabled = true;
            _gravity.enabled = true;
            _isTravel = false;
            _player.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if player makes collision with somting, travel is true than.
        if (collision.CompareTag("Player"))
        {
            OnLaunch?.Invoke(travelIntent);
            _gravity.ResetGravity();
            _gravity.enabled = false;
            _movementData.WorldMiddle = _nextPlanet;
            _travelTime = Time.time;
            _isTravel =  true;
            _player.transform.SetParent(transform);
            _capsuleCollider.enabled = false;
            _playerMovement.enabled = false;
        }
    }
}
