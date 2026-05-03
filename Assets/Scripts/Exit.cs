using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string destinationSceneName;
    [SerializeField] private string destinationSpawnId;
    [SerializeField] public string SpawnId;

    private bool _isTriggered = false;
    public bool _spawnedInside = false;
    private bool _isLocked = false;

    public void Lock() => _isLocked = true;
    public void Unlock() => _isLocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered || _spawnedInside || _isLocked) return;

        // Only the player should trigger room transitions
        if (!other.CompareTag("Player")) return;

        if (RoomTransitionManager.Instance == null)
        {
            Debug.LogError("No RoomTransitionManager found in scene/persistent objects.");
            return;
        }

        _isTriggered = true;
        RoomTransitionManager.Instance.GoToRoom(destinationSceneName, destinationSpawnId);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_spawnedInside)
        {
            _spawnedInside = false;
        }
    }
}