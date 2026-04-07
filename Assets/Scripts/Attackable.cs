//Written by Zane Pederson

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Attackable : MonoBehaviour
{
    [SerializeField]
    int health;

    //for sprite flash red
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Renderer rend;

    private int maxHealth;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Attacked(int damage)
    {
        StartCoroutine(FlashRoutine());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            health = 5; //TODO: IF CHANGE PLAYER HEALTH CHANGE THIS TOO
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
