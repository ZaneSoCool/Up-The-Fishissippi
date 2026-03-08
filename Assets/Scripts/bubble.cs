//Written by Zane Pederson

using UnityEngine;

public class bubble : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 3.0f;

    Rigidbody2D playerRB;

    // called when player bumps into bubble
    void OnTriggerEnter2D(Collider2D col)
    {
        playerRB = col.gameObject.GetComponent<Rigidbody2D>();
        if (col.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
        }
    }
}



