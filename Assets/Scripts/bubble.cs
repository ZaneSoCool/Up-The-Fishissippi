//Written by Zane Pederson and Nathan Gumagay

using System;
using UnityEngine;
using UnityEngine.AI;

public class bubble : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 3.0f;

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
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        playerRB = col.gameObject.GetComponent<Rigidbody2D>();
        if (col.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
            Debug.Log("Calling SetTrigger Pop");
            bubbleAnimator.SetTrigger("Pop");
        }
    }

    public void bubbleAttacked()
    {
        if (attack.Die() == this)
        {
            Debug.Log("Calling SetTrigger Death");
            bubbleCollider.enabled = false;
            bubbleAnimator.SetTrigger("Death");
            Destroy(this);
        }
    }

    public void bubblePop()
    {
        Debug.Log("Pop animation triggered!");
    }

    public void bubbleDeath()
    {
        Debug.Log("Death animation triggered!");
    }
}



