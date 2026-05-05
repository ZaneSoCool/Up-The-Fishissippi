using UnityEngine;

public class EnemyBodyCollider : MonoBehaviour
{
    [SerializeField]
    private TerritorialFish fish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        fish.HandlePlayerBodyContact(collision);
    }
}
