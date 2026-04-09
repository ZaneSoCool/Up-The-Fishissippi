using UnityEngine;

// Attach to the InteractZone child of a Checkpoint.
// Tells the parent Checkpoint when the player is close enough to interact.
public class InteractZoneTrigger : MonoBehaviour
{
    private Checkpoint checkpoint;

    private void Awake()
    {
        checkpoint = GetComponentInParent<Checkpoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            checkpoint.SetInInteractZone(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            checkpoint.SetInInteractZone(false);
    }
}
