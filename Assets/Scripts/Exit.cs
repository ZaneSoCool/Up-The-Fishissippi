using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string destinationSceneName;
    [SerializeField] private string destinationSpawnId;
    [SerializeField] public string SpawnId;

    private bool _isTriggered = false;
    public bool _spawnedInside = false; //set to true if player is to be spawned at this trigger
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered  || _spawnedInside) return;

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