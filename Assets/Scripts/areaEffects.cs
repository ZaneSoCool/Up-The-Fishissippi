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
    private GameObject backgroundWallTiles; 
    [SerializeField]
    private GameObject backgroundWallTiles2; 
    [SerializeField]
    private Color area2backColor;
    [SerializeField]
    private Color area2wallColor;
    [SerializeField]
    private Color area2backWallColor;
    [SerializeField]
    private Color area2backWall2Color;

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

        Tilemap backgroundTilemap = backgroundWallTiles.GetComponent<Tilemap>();
        backgroundTilemap.color = area2backWallColor;

        Tilemap backgroundTilemap2 = backgroundWallTiles2.GetComponent<Tilemap>();
        backgroundTilemap2.color = area2backWall2Color;
    }
}
