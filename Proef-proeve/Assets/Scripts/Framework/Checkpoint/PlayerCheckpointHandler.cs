using UnityEngine;

public class PlayerCheckpointHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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
