using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private string collectableId;

    void Start()
    {
        // Should destroy the object if it's already collected.
        if (CollectableManager.Instance != null && CollectableManager.Instance.IsCollected(collectableId))
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableManager.Instance?.Collect(collectableId);
            Activate();
        }
    }

    void Activate()
    {
        // Implement collectable-specific behavior here.
        Debug.Log("Collectable activated!");
    }
}
