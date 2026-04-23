using UnityEngine;

public class Collectable : MonoBehaviour
{
    private string collectableId = "1";

    public string ID => collectableId;

    void Start()
    {
        //collectableId = GETID(HOW!); //NEED TO GET ID THAT IS UNIQUE TO EACH INSTANCE BUT IS CONSISTENT

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
