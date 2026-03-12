using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    public static CheckpointManager Instance { get; private set; }
    private bool hasCheckpoint = false;
    private string checkpointScene;
    private Vector3 checkpointPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckpoint(string scene, Vector3 position)
    {
        hasCheckpoint = true;
        checkpointScene = scene;
        checkpointPosition = position;
    }
    public void Respawn()
    {
        if (hasCheckpoint)
        {
            if (SceneManager.GetActiveScene().name != checkpointScene)
            {
                SceneManager.LoadScene(checkpointScene);
            }
            player.transform.position = checkpointPosition;
            Debug.Log("Respawning at checkpoint: " + checkpointScene + " at position: " + checkpointPosition);
        }
        else
        {
            // No checkpoint set, respawn at the default spawn point
            Debug.Log("No checkpoint set, respawning at default spawn point.");
        }
    }
}
