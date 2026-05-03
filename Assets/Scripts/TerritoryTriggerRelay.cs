using UnityEngine;

public class TerritoryTriggerRelay : MonoBehaviour
{
    private TerritorialFish enemy;
    public string playerTag = "Player";

    public void Initialize(TerritorialFish enemyZone)
    {
        enemy = enemyZone;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        enemy.PlayerEnteredTerritory(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        enemy.PlayerExitedTerritory(other.transform);
    }
}
