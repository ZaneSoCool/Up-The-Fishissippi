using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class CreateFishTiles
{
    public static void Execute()
    {
        Sprite idleSprite  = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/EnemyFish/Enemy Fish Idle.png");
        Sprite swimSprite  = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/EnemyFish/Enemy Fish Swim 1.png");

        if (idleSprite == null || swimSprite == null)
        {
            Debug.LogError("CreateFishTiles: could not find enemy fish sprites. Check paths.");
            return;
        }

        string[] colorNames = { "Red", "Blue", "Green", "Yellow", "Purple", "Orange" };
        Color[]  colors     =
        {
            new Color(1.00f, 0.20f, 0.20f),
            new Color(0.20f, 0.40f, 1.00f),
            new Color(0.20f, 0.80f, 0.20f),
            new Color(1.00f, 0.90f, 0.10f),
            new Color(0.70f, 0.20f, 1.00f),
            new Color(1.00f, 0.60f, 0.10f),
        };

        Directory.CreateDirectory("Assets/Tilesets/Tiles/Fish");

        for (int i = 0; i < colorNames.Length; i++)
        {
            CreateTile($"Assets/Tilesets/Tiles/Fish/FishStart_{colorNames[i]}.asset", idleSprite, colors[i]);
            CreateTile($"Assets/Tilesets/Tiles/Fish/FishEnd_{colorNames[i]}.asset",   swimSprite, colors[i]);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("CreateFishTiles: created 12 fish marker tiles in Assets/Tilesets/Tiles/Fish/");
    }

    static void CreateTile(string path, Sprite sprite, Color color)
    {
        Tile tile   = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color  = color;
        AssetDatabase.CreateAsset(tile, path);
    }
}
