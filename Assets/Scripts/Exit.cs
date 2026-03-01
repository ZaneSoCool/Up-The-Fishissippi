using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string destinationSceneName;
    [SerializeField] private string destinationSpawnId;

    private bool _isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered) return;

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
}