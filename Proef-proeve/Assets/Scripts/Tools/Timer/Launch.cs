using UnityEngine;

public class Launch : MonoBehaviour
{
    PlayerGravity _gravity;
    MovementData _movementData;
    [SerializeField] private Transform _startLocation;
    [SerializeField] private Transform _endLocation;
    [SerializeField]private Transform _nextPlanet;

    [SerializeField] private GameObject _player;

    [SerializeField] private float _landRotation;
    [SerializeField] private float _speed;
    [SerializeField] private float _travelTime;

    private CapsuleCollider _capsuleCollider;
    [SerializeField] private MeshRenderer _renderer;

    private float _elapsedTime;
    private float _planetDistance;
    private float _journey;

    private bool _isTravel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _movementData = _player.GetComponent<MovementData>();
        _gravity = _player.GetComponent<PlayerGravity>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _planetDistance = Vector3.Distance(_startLocation.position, _endLocation.position);
    }

    // Update is called once per frame
    private void Update()
    {
    // Check if travel is true, and if it is Gameobject start traveling.
        if (_isTravel)
        {
            _elapsedTime = (Time.time - _travelTime) * _speed;
            Traveling();
        }
    }

    private void Traveling()
    {
        // check the distandce between planets and devide it with the time it will take to get there.
        _journey = _elapsedTime / _planetDistance;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(_landRotation, 0, 0), _journey);
        transform.position = Vector3.Lerp(_startLocation.position, _endLocation.position, _journey);
        // reset the journey if it is done.
        if (_journey >= 1f)
        {
            Debug.Log("land");
            _gravity.enabled = true;
            _isTravel = false;
            _renderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if player makes collision with somting, travel is true than.
        if (collision.gameObject == _player)
        {
            _gravity.ResetGravity();
            _gravity.enabled = false;
            _movementData.WorldMiddle = _nextPlanet;
            _travelTime = Time.time;
            _isTravel =  true;
            _player.transform.SetParent(transform);
            _renderer.enabled = false;
            _capsuleCollider.enabled = false;
        }
    }
}
