//Written by Zane Pederson and Nathan Gumagay

using System;
using UnityEngine;
using UnityEngine.AI;

public class bubble : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 3.0f;

    [SerializeField] AudioClip bounceClip;
    [SerializeField] AudioClip popClip;

    Rigidbody2D playerRB;

    Animator bubbleAnimator;

    Attackable attack;

    private CircleCollider2D bubbleCollider;

    // called when player bumps into bubble

    void Start()
    {
        bubbleAnimator = GetComponent<Animator>();
        bubbleCollider = GetComponent<CircleCollider2D>();
        attack = GetComponent<Attackable>();
        attack.onDeath = bubbleAttacked;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        playerRB = col.gameObject.GetComponent<Rigidbody2D>();
        if (col.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
            if (bounceClip != null) AudioSource.PlayClipAtPoint(bounceClip, transform.position);
            bubbleAnimator.SetTrigger("Pop");
        }
    }

    private bool _isDead = false;

    public void bubbleAttacked()
    {
        if (_isDead) return;
        _isDead = true;
        if (popClip != null) AudioSource.PlayClipAtPoint(popClip, transform.position);
        bubbleCollider.enabled = false;
        bubbleAnimator.SetTrigger("Death");
    }

    public void bubblePop()
    {
        Debug.Log("Pop animation triggered!");
    }

    public void bubbleDeath()
    {
        Destroy(gameObject);
    }
}



