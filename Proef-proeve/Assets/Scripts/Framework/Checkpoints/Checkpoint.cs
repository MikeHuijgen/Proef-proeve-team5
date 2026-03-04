using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] Particles;

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
        foreach (var ps in Particles)
        {
            ps.Play();
        }
    }
}