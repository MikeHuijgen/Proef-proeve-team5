using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public MovementData MovementData;

    private void Awake()
    {
        MovementData = GetComponent<MovementData>();
    }
}
