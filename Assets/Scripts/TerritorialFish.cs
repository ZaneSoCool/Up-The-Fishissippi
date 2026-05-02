using UnityEngine;

public class TerritorialFish : Territory
{
    public float alertDistance = 3f;
    public float chaseDistance = 1.5f;

    protected override void OnPlayerEnterTerritory(Transform player)
    {
        base.OnPlayerEnterTerritory(player);
        // e.g., flip enemy sprite to face player, play alert sound
    }

    protected override void OnPlayerExitTerritory(Transform player)
    {
        base.OnPlayerExitTerritory(player);
        // e.g., return enemy to patrol state
    }

    protected override void OnPlayerMoved(Transform player, Vector2 localPos, float distance)
    {
        if (distance < chaseDistance)
        {
            Debug.Log($"Player too close! Chasing! Distance: {distance:F2}");
            // e.g., trigger chase behavior, deal damage, etc.
        }
        else if (distance < alertDistance)
        {
            Debug.Log($"Player detected! Alerting! Distance: {distance:F2}");
            // e.g., move toward player slowly
        }
    }
}
