using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CheckpointSystem.Instance.SetCheckpoint(this);
    }

    public void Activate()
    {
        PlayParticles();
    }

    private void PlayParticles()
    {
        foreach (var ps in particles)
        {
            ps.Play();
        }
    }
}