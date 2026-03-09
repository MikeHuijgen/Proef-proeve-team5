using UnityEngine;

public class MovementData : MonoBehaviour
{
    public CharacterController CharacterController;
    public Transform WorldMiddle;
    public Transform Camera;
    public Transform PlayerBody;
    public Vector3 Velocity { get; private set; }
    
    private Vector3 _lastPosition;
    
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();

        _lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = currentPosition;
    }
}
