using UnityEngine;

public class EnemyBodyCollider : MonoBehaviour
{
    private TerritorialFish fish;

    public void Initialize(TerritorialFish territorialFish)
    {
        fish = territorialFish;
        Debug.Log("EnemyBodyCollider initialized successfully!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fish == null)
        {
            Debug.LogError("fish is null! Initialize() was not called.");
            return;
        }

        if (!collision.gameObject.CompareTag("Player")) return;
        fish.HandlePlayerBodyContact(collision);
    }
}
