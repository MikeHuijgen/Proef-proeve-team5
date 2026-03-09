using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public MovementData MovementData { get; private set; }

    private void Awake()
    {
        MovementData = GetComponent<MovementData>();
    }
}
