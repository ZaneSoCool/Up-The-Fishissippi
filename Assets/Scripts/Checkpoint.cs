using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField]
    private string checkpointScene;
    private Vector3 checkpointPosition;
    private bool isActive = false;
    private float activationDelay = 0.2f; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkpointPosition = transform.position + new Vector3(0, 5, 0); // Adjust the checkpoint position to be slightly above the checkpoint object
            CheckpointManager.Instance.SetCheckpoint(checkpointScene, checkpointPosition);
            isActive = true;
            Debug.Log("Checkpoint activated at scene: " + checkpointScene + " at position: " + checkpointPosition);
        }
    }
}
