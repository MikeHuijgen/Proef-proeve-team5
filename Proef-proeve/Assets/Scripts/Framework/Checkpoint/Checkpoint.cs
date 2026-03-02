using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticle;

    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only respond to player
        if (!other.CompareTag("Player"))
            return;

        // If already activated, do nothing
        if (isActivated)
            return;

        ActivateCheckpoint();
    }

    private void ActivateCheckpoint()
    {
        isActivated = true;

        // Tell system this is now the active checkpoint
        CheckpointSystem.Instance.SetCheckpoint(transform);

        // Play particle once
        if (checkpointParticle != null)
            checkpointParticle.Play();

        Debug.Log("Checkpoint Activated: " + gameObject.name);
    }
}