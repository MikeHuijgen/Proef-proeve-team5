using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    [Header("References")]
    [SerializeField] private Transform Player;
    [SerializeField] private Transform DefaultSpawnPoint;

    private Checkpoint _currentCheckpoint;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCheckpoint(Checkpoint newCheckpoint)
    {
        if (_currentCheckpoint == newCheckpoint)
            return;

        _currentCheckpoint = newCheckpoint;

        _currentCheckpoint.Activate();
    }

    public void RespawnPlayer()
    {
        Transform spawnPoint = DefaultSpawnPoint;

        if (_currentCheckpoint != null)
            spawnPoint = _currentCheckpoint.transform;

        CharacterController controller = Player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        Player.position = spawnPoint.position;
        Player.rotation = spawnPoint.rotation;

        if (controller != null)
            controller.enabled = true;
    }
}