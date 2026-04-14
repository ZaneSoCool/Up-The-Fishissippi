//Written by Zane Pederson and Nathan Gumagay

using UnityEngine;

public class bubble : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 3.0f;

    Rigidbody2D playerRB;

    Animator bubbleAnimator;

    // called when player bumps into bubble

    void Start()
    {
        bubbleAnimator = GetComponent<Animator>();
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

    public void bubblePop()
    {
        Debug.Log("Pop animation triggered!");
    }
}



