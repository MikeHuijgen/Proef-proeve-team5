using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static event Action OnDie;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        OnDie?.Invoke();
        CheckpointSystem.Instance.RespawnPlayer();        
    }
}