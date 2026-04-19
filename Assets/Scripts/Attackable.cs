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

    private int maxHealth;

    public bool canBeTailThwapped = true;
    public bool canBeSkullBashed = true;

    void Start()
    {
        rend = GetComponent<Renderer>();
        maxHealth = health;
    }

    public void Attacked(int damage)
    {
        // Don't take damage during scene transitions
        if (RoomTransitionManager.Instance != null && RoomTransitionManager.Instance.IsTransitioning) return;

        StartCoroutine(FlashRoutine());
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

    public void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            rend.material.color = rend.material.color;
            RoomTransitionManager.Instance.RespawnAtDefault();
        } else
        {
            Destroy(gameObject);
        }
    }
    IEnumerator FlashRoutine() {
        Color ogColor = rend.material.color;
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = ogColor;
    }
}
