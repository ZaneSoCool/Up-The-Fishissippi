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

    private GameObject canvas;

    private void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<player>();
        interactAction = InputSystem.actions.FindAction("Interact");
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Check for interact input while player is in the interact zone
        if (playerInInteractZone && interactAction != null && interactAction.WasPressedThisFrame() && sprite.enabled == true)
            Activate();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInInteractZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInInteractZone = false;
    }

    private void Activate()
    {
        sprite.enabled = false;
        playerScript.hasSkullBash = true;
        playerScript.hasTailThwap = true;
        canvas.SetActive(true);
        Destroy(gameObject, 5.0f);
    }
}
