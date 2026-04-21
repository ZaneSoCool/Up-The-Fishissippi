using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer visuals;
    [SerializeField] private CanvasGroup interactPrompt;
    [SerializeField] private float promptFadeDuration = 0.2f;

    [Header("Dialogue")]
    [SerializeField] private CanvasGroup dialogCanvasGroup;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] [TextArea] private string[] firstMeetLines;
    [SerializeField] [TextArea] private string[] respawnLines;
    [SerializeField] private float dialogAlpha = 0.85f;
    [SerializeField] private float dialogFadeDuration = 0.25f;

    private Transform player;
    private bool playerIsNear = false;
    private bool playerInInteractZone = false;
    private InputAction interactAction;
    private Coroutine fadeCoroutine;

    private bool hasActivated = false;
    private bool isDialogOpen = false;
    private int dialogIndex = 0;
    private string[] _activeLines;
    private player playerScript;
    private Coroutine dialogFadeCoroutine;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        playerScript = player?.GetComponent<player>();
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactPrompt != null)
        {
            interactPrompt.alpha = 0f;
            interactPrompt.gameObject.SetActive(false);
        }
        if (dialogCanvasGroup != null)
        {
            dialogCanvasGroup.alpha = 0f;
            dialogCanvasGroup.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Flip Jaune to face the player
        if (playerIsNear && player != null && visuals != null)
            visuals.flipX = player.position.x > transform.position.x;

        // Check for interact input while player is in the interact zone
        if (playerInInteractZone && interactAction != null && interactAction.WasPressedThisFrame())
        {
            if (isDialogOpen)
                AdvanceDialog();
            else
                Activate();
        }
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
        Vector3 spawnPos = transform.position + new Vector3(0, 1, 0);
        RoomTransitionManager.Instance.SetCheckpoint(currentScene, spawnPos);

        player?.GetComponent<Attackable>()?.ResetHealth();

        Debug.Log("Checkpoint activated: " + currentScene + " @ " + spawnPos);

        if (dialogCanvasGroup != null && firstMeetLines.Length > 0)
        {
            bool isRespawn = RoomTransitionManager.Instance != null && RoomTransitionManager.Instance.WasCheckpointRespawn;
            if (isRespawn && respawnLines.Length > 0)
            {
                RoomTransitionManager.Instance.ClearRespawnFlag();
                hasActivated = true;
                dialogIndex = 0;
                _activeLines = respawnLines;
            }
            else if (!hasActivated)
            {
                if (isRespawn) RoomTransitionManager.Instance?.ClearRespawnFlag();
                hasActivated = true;
                dialogIndex = 0;
                _activeLines = firstMeetLines;
            }
            else
            {
                // Repeat visit: jump straight to last line
                dialogIndex = firstMeetLines.Length - 1;
                _activeLines = firstMeetLines;
            }
            dialogText.text = _activeLines[dialogIndex];
            isDialogOpen = true;
            if (animator != null) animator.SetBool("IsSpeaking", true);
            if (playerScript != null) playerScript.inputEnabled = false;
            if (dialogFadeCoroutine != null) StopCoroutine(dialogFadeCoroutine);
            dialogFadeCoroutine = StartCoroutine(FadeDialog(dialogAlpha));
        }
        else
        {
            StartCoroutine(SaveFeedback());
        }
    }

    private void AdvanceDialog()
    {
        dialogIndex++;
        if (dialogIndex < _activeLines.Length)
        {
            dialogText.text = _activeLines[dialogIndex];
        }
        else
        {
            isDialogOpen = false;
            if (animator != null) animator.SetBool("IsSpeaking", false);
            if (playerScript != null) playerScript.inputEnabled = true;
            if (dialogFadeCoroutine != null) StopCoroutine(dialogFadeCoroutine);
            dialogFadeCoroutine = StartCoroutine(FadeDialog(0f));
            StartCoroutine(SaveFeedback());
        }
    }

    private IEnumerator FadeDialog(float target)
    {
        dialogCanvasGroup.gameObject.SetActive(true);
        float start = dialogCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < dialogFadeDuration)
        {
            elapsed += Time.deltaTime;
            dialogCanvasGroup.alpha = Mathf.Lerp(start, target, elapsed / dialogFadeDuration);
            yield return null;
        }

        dialogCanvasGroup.alpha = target;
        if (target == 0f)
            dialogCanvasGroup.gameObject.SetActive(false);
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
