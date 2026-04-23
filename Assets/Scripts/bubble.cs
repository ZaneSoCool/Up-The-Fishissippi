//Written by Zane Pederson and Nathan Gumagay

using System;
using UnityEngine;

public class bubble : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 3.0f;

    Rigidbody2D playerRB;

    Animator bubbleAnimator;

    Attackable attack;

    //Collider2D bubbleCollider;

    //Boolean colliderIsActive;

    // called when player bumps into bubble

    void Start()
    {
        bubbleAnimator = GetComponent<Animator>();
        //bubbleCollider = GetComponent<Collider2D>();
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

    public void Attacked()
    {
        if (attack.Die() == this)
        {
            Debug.Log("Calling SetTrigger Death");
            //bubbleCollider.
            bubbleAnimator.SetTrigger("Death");
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

    //public void colliderActive()
    //{
    //}
}



