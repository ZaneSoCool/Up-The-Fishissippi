using UnityEngine;
using UnityEngine.Tilemaps;

public class areaEffects : MonoBehaviour
{
    [SerializeField]
    private bool isArea2;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject wallTiles; 
    [SerializeField]
    private GameObject hiddenWallTiles; 
    [SerializeField]
    private Color area2backColor;
    [SerializeField]
    private Color area2wallColor;

    void Start()
    {
        if (!isArea2) return;

        //sets up area 2 colors
        SpriteRenderer backgroundRenderer = background.GetComponent<SpriteRenderer>();
        backgroundRenderer.color = area2backColor;

        Tilemap tilemap = wallTiles.GetComponent<Tilemap>();
        tilemap.color = area2wallColor;

        Tilemap hiddenTilemap = hiddenWallTiles.GetComponent<Tilemap>();
        hiddenTilemap.color = area2wallColor;
    }
}
