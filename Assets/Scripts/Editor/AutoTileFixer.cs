using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Reflection;

public static class AutoTileFixer
{
    [MenuItem("Tools/List AutoTile Masks")]
    static void ListMasks()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is AutoTile autoTile)
            {
                var field = typeof(AutoTile).GetField("m_AutoTileDictionary",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var dict = field.GetValue(autoTile) as System.Collections.IDictionary;
                
                var masks = new System.Collections.Generic.List<uint>();
                foreach (System.Collections.DictionaryEntry entry in dict)
                    masks.Add((uint)entry.Key);
                masks.Sort();
                
                Debug.Log($"Masks present: {string.Join(", ", masks)}");
                Debug.Log($"Default sprite: {(autoTile.m_DefaultSprite != null ? autoTile.m_DefaultSprite.name : "NULL")}");
            }
        }
    }
    
    [MenuItem("Tools/Validate Selected AutoTile")]
    static void ValidateSelected()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is AutoTile autoTile)
            {
                autoTile.Validate();
                EditorUtility.SetDirty(autoTile);
                Debug.Log($"Validated {autoTile.name}");
            }
        }
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Inspect Selected AutoTile")]
    static void InspectSelected()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is AutoTile autoTile)
            {
                var field = typeof(AutoTile).GetField("m_AutoTileDictionary",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var dict = field.GetValue(autoTile) as System.Collections.IDictionary;
                
                Debug.Log($"=== {autoTile.name} has {dict.Count} mask entries ===");
                foreach (System.Collections.DictionaryEntry entry in dict)
                {
                    var data = entry.Value;
                    var spriteListField = data.GetType().GetField("spriteList");
                    var spriteList = spriteListField.GetValue(data) as System.Collections.IList;
                    
                    int nullCount = 0;
                    for (int i = 0; i < spriteList.Count; i++)
                    {
                        if (spriteList[i] == null || (spriteList[i] as Object) == null)
                            nullCount++;
                    }
                    
                    if (nullCount > 0 || spriteList.Count == 0)
                    {
                        Debug.LogWarning($"Mask {entry.Key}: {spriteList.Count} sprites, {nullCount} NULL");
                    }
                }
            }
        }
    }
}