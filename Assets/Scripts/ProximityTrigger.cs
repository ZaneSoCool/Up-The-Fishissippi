using UnityEngine;

// Attach to the ProximityZone child of a Checkpoint.
// Tells the parent Checkpoint when the player is nearby so Jaune animates.
public class ProximityTrigger : MonoBehaviour
{
    private Checkpoint checkpoint;

    private void Awake()
    {
        checkpoint = GetComponentInParent<Checkpoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            checkpoint.SetNear(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            checkpoint.SetNear(false);
    }
}
