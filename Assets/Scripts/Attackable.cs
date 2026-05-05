//Written by Zane Pederson

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Attackable : MonoBehaviour
{
    [SerializeField]
    int health;

    public int CurrentHealth => health;

    //for sprite flash red
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Renderer rend;
    private Color _originalColor;
    private Coroutine _flashCoroutine;

    private int maxHealth;

    public bool canBeTailThwapped = true;
    public bool canBeSkullBashed = true;

    public bool parentDestroyOnDeath = false;
    // Optional: set to intercept death instead of auto-destroying
    public System.Action onDeath = null;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (rend == null) rend = GetComponent<Renderer>();
        maxHealth = health;
        if (rend != null && rend.material.HasProperty("_Color"))
            _originalColor = rend.material.color;
    }

    public void Attacked(int damage)
    {
        // Don't take damage during scene transitions
        if (RoomTransitionManager.Instance != null && RoomTransitionManager.Instance.IsTransitioning) return;

        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(FlashRoutine());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    //Added a return statement of game object; In order to catch whcih game object died
    public GameObject Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            rend.material.color = rend.material.color;
            RoomTransitionManager.Instance.RespawnAtDefault();

            return null;
        }
        else
        {
            if (gameObject.CompareTag("Bubble") || gameObject.CompareTag("BigBubble"))
            {
                return gameObject;
            }
            
            if (onDeath != null)
            {
                onDeath.Invoke();
                return null;
            }

            if (parentDestroyOnDeath)
            {
                Destroy(transform.parent.gameObject);
            } else
            {
                Destroy(gameObject);
            }

            return null;
        }
    }
    IEnumerator FlashRoutine()
    {
        if (rend == null || !rend.material.HasProperty("_Color")) yield break;
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = _originalColor;
    }
}
