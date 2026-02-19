using UnityEngine;

public class PlayerCheckpointHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collided with: " + other.name);
        if (other.CompareTag("Checkpoint"))
        {
            CheckpointSystem.Instance.SetCheckpoint(other.transform);
        }

        if (other.CompareTag("Death"))
        {
            CheckpointSystem.Instance.RespawnPlayer();
        }
    }
}
