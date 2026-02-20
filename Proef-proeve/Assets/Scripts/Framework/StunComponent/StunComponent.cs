using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class StunComponent : MonoBehaviour
{
    [SerializeField] private StateIntentData stunIntent;

    public static event Action<StateIntentData> OnPlayerCollision;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnPlayerCollision?.Invoke(stunIntent);
    }
}
