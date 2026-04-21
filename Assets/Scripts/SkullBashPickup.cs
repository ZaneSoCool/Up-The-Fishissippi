using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkullBashPickup : MonoBehaviour
{
    private player playerScript;
    private bool playerInInteractZone = false;
    private InputAction interactAction;
    private SpriteRenderer sprite;
    private Coroutine fadeCoroutine;
    [SerializeField] private CanvasGroup interactPrompt;
    [SerializeField] private float promptFadeDuration = 0.2f;
    

    private GameObject canvas;

    private void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<player>();
        interactAction = InputSystem.actions.FindAction("Interact");
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();

        if (interactPrompt != null)
        {
            interactPrompt.alpha = 0f;
            interactPrompt.gameObject.SetActive(false);
        }
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

    private void Update()
    {
        // Check for interact input while player is in the interact zone
        if (playerInInteractZone && interactAction != null && interactAction.WasPressedThisFrame() && sprite.enabled == true)
            Activate();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInInteractZone = true;
        if (interactPrompt == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadePrompt(1f));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInInteractZone = false;

        if (interactPrompt == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadePrompt(0f));
    }

    private void Activate()
    {
        sprite.enabled = false;
        playerScript.hasSkullBash = true;
        playerScript.hasTailThwap = true;
        canvas.SetActive(true);

        if (interactPrompt != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadePrompt(0f));
        }
        Destroy(interactPrompt.gameObject, 1.0f);

        Destroy(gameObject, 5.0f);
    }
}
