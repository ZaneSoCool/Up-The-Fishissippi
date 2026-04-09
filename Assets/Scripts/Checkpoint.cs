using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string checkpointScene;
    [SerializeField] private SpriteRenderer visuals;

    private Transform player;
    private bool playerIsNear = false;
    private bool playerInInteractZone = false;
    private InputAction interactAction;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void OnEnable()
    {
        // Subscribed in Start once interactAction is found; re-subscribe on enable
    }

    private void Update()
    {
        // Flip Jaune to face the player
        if (playerIsNear && player != null && visuals != null)
            visuals.flipX = player.position.x > transform.position.x;

        // Check for interact input while player is in the interact zone
        if (playerInInteractZone && interactAction != null && interactAction.WasPressedThisFrame())
            Activate();
    }

    // Called by ProximityTrigger child
    public void SetNear(bool near)
    {
        playerIsNear = near;
        if (animator != null)
            animator.SetBool("IsNear", near);
    }

    // Called by InteractZoneTrigger child
    public void SetInInteractZone(bool inZone)
    {
        playerInInteractZone = inZone;
    }

    private void Activate()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, 5, 0);
        RoomTransitionManager.Instance.SetCheckpoint(checkpointScene, spawnPos);
        Debug.Log("Checkpoint activated: " + checkpointScene + " @ " + spawnPos);
        StartCoroutine(SaveFeedback());
        // TODO: trigger Jaune dialogue here
    }

    // Pops Jaune in as visual confirmation, then pops back out if player is still near
    private IEnumerator SaveFeedback()
    {
        if (animator == null) yield break;

        animator.SetBool("IsNear", false);
        yield return new WaitForSeconds(1f);

        if (playerIsNear)
            animator.SetBool("IsNear", true);
    }
}
