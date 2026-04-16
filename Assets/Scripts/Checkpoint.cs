using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer visuals;
    [SerializeField] private CanvasGroup interactPrompt;
    [SerializeField] private float promptFadeDuration = 0.2f;

    private Transform player;
    private bool playerIsNear = false;
    private bool playerInInteractZone = false;
    private InputAction interactAction;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactPrompt != null)
        {
            interactPrompt.alpha = 0f;
            interactPrompt.gameObject.SetActive(false);
        }
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
        if (interactPrompt == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadePrompt(inZone ? 1f : 0f));
    }

    private IEnumerator FadePrompt(float target)
    {
        interactPrompt.gameObject.SetActive(true);
        float start = interactPrompt.alpha;
        float elapsed = 0f;

        while (elapsed < promptFadeDuration)
        {
            elapsed += Time.deltaTime;
            interactPrompt.alpha = Mathf.Lerp(start, target, elapsed / promptFadeDuration);
            yield return null;
        }

        interactPrompt.alpha = target;
        if (target == 0f)
            interactPrompt.gameObject.SetActive(false);
    }

    private void Activate()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Vector3 spawnPos = transform.position + new Vector3(0, 1, 0); // Adjust spawn position as needed
        RoomTransitionManager.Instance.SetCheckpoint(currentScene, spawnPos);

        // Restore player health on checkpoint activation
        player?.GetComponent<Attackable>()?.ResetHealth();

        Debug.Log("Checkpoint activated: " + currentScene + " @ " + spawnPos);
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
