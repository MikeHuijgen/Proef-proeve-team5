using UnityEngine;

public class StunComponent : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerStunRequestHandler>(out var playerStunRequestHandler)) return;
        playerStunRequestHandler.RequestStun();
    }
}
