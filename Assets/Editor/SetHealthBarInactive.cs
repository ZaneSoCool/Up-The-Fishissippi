using UnityEngine;
using UnityEditor;

public class SetHealthBarInactive
{
    public static void Execute()
    {
        GameObject obj = GameObject.Find("BossHealthBar");
        if (obj != null)
        {
            obj.SetActive(false);
            EditorUtility.SetDirty(obj);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(obj.scene);
            Debug.Log("BossHealthBar set inactive.");
        }
        else
        {
            Debug.LogWarning("BossHealthBar not found.");
        }
    }
}
