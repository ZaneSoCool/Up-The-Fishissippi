using UnityEditor;
using UnityEngine;

public class FixEnemyFishPrefab
{
    public static void Execute()
    {
        string prefabPath = "Assets/Prefabs/Enemy_Fish.prefab";
        GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);

        // Remove the broken child sprite frame objects
        Transform swim1 = prefab.transform.Find("Enemy Fish Swim 1_0");
        Transform swim2 = prefab.transform.Find("Enemy Fish Swim 2_0");
        if (swim1 != null) Object.DestroyImmediate(swim1.gameObject);
        if (swim2 != null) Object.DestroyImmediate(swim2.gameObject);

        // Fix the collider — it must be a trigger for OnTriggerEnter2D to fire
        CircleCollider2D col = prefab.GetComponent<CircleCollider2D>();
        if (col != null) col.isTrigger = true;

        PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefab);

        Debug.Log("FixEnemyFishPrefab: done.");
    }
}
