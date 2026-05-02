using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class Territory : MonoBehaviour
{
    [Header("Territory Settings")]
    public string playerTag = "Player";
    public bool drawGizmos = true;
    public Color territoryColor = new Color(0f, 1f, 0f, 0.2f);

    [Header("Tracking")]
    public float trackingInterval = 0.1f; // How often to track position (seconds)

    private Transform player;
    private float trackingTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        trackingTimer += Time.deltaTime;

        if (trackingTimer >= trackingInterval)
        {
            trackingTimer = 0f;
            TrackPlayer();
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(playerTag)) return;

        player = col.transform;

        //OnPlayerEnterTerritory();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag(playerTag)) return;

        OnPlayerExitTerritory(player);
        player = null;
    }

    private void TrackPlayer()
    {
        Vector2 worldPos2D = new Vector2(player.position.x, player.position.y);
        Vector2 localPos2D = transform.InverseTransformPoint(player.position);

        float distanceToCenter = Vector2.Distance(worldPos2D,
            new Vector2(transform.position.x, transform.position.y));

        OnPlayerMoved(player, localPos2D, distanceToCenter);
    }

    protected virtual void OnPlayerEnterTerritory(Transform player)
    {
        Debug.Log($"[{gameObject.name}] Player '{player.name}' ENTERED territory.");
    }

    protected virtual void OnPlayerExitTerritory(Transform player)
    {
        Debug.Log($"[{gameObject.name}] Player '{player.name}' EXITED territory.");
    }

    protected virtual void OnPlayerMoved(Transform player, Vector2 localPos, float distance)
    {
        // Override to react to player movement
    }

    public bool IsPlayerInTerritory() => player != null;
}
