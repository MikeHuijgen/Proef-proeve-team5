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
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentCheckpoint = defaultSpawnPoint;
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        // Prevent redundant updates
        if (currentCheckpoint == newCheckpoint)
            return;

        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Updated: " + newCheckpoint.name);
    }

    public void RespawnPlayer()
    {
        if (currentCheckpoint == null)
            currentCheckpoint = defaultSpawnPoint;

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = currentCheckpoint.position;
        player.rotation = currentCheckpoint.rotation;

        if (controller != null)
            controller.enabled = true;

        Debug.Log("Player Respawned");
    }
}