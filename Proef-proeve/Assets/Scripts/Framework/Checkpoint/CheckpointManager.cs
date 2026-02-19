using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform defaultSpawnPoint;

    private Transform currentCheckpoint;

    private void Awake()
    {
        // Simple singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // If no checkpoint touched yet, use default spawn
        currentCheckpoint = defaultSpawnPoint;
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Updated: " + newCheckpoint.name);
    }

    public void RespawnPlayer()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = currentCheckpoint.position;
        player.rotation = currentCheckpoint.rotation;

        if (controller != null)
            controller.enabled = true;
    }
}
