using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This component spawns resources based on a tilemap. Each tile in the tilemap corresponds to a resource prefab.

public class ResourceSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ResourceMapping
    {
        public string name;                 // just for readability in Inspector
        public TileBase markerTile;         // tile painted in ResourceMarkers tilemap
        public GameObject resourcePrefab;     // prefab to spawn
        public Vector3 worldOffset;         // e.g. bubble top-left anchor = (0.5, -0.5, 0)
        public bool clearMarkerAfterSpawn = true;
    }

    [Header("References")]
    [SerializeField] private Tilemap resourceMarkerTilemap;
    [SerializeField] private Transform resourcesParent;

    [Header("Marker -> Prefab mappings")]
    [SerializeField] private List<ResourceMapping> mappings = new List<ResourceMapping>();

    private void Start()
    {
        SpawnAllResources();
    }

    public void SpawnAllResources()
    {
        if (resourceMarkerTilemap == null)
        {
            Debug.LogError("ResourceSpawner: resourceMarkerTilemap is not assigned.");
            return;
        }

        if (resourcesParent == null)
        {
            Debug.LogError("ResourceSpawner: resourcesParent is not assigned.");
            return;
        }

        // Build a quick lookup dictionary
        Dictionary<TileBase, ResourceMapping> lookup = new Dictionary<TileBase, ResourceMapping>();
        foreach (var m in mappings)
        {
            if (m.markerTile != null && m.resourcePrefab != null && !lookup.ContainsKey(m.markerTile))
            {
                lookup.Add(m.markerTile, m);
            }
        }

        // Tighten bounds to used cells only
        resourceMarkerTilemap.CompressBounds();
        BoundsInt bounds = resourceMarkerTilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            TileBase tile = resourceMarkerTilemap.GetTile(cellPos);
            if (tile == null) continue;

            if (!lookup.TryGetValue(tile, out ResourceMapping mapping))
                continue;

            Vector3 spawnPos = resourceMarkerTilemap.GetCellCenterWorld(cellPos) + mapping.worldOffset;

            Instantiate(mapping.resourcePrefab, spawnPos, Quaternion.identity, resourcesParent);

            if (mapping.clearMarkerAfterSpawn)
            {
                resourceMarkerTilemap.SetTile(cellPos, null);
            }
        }
    }
}