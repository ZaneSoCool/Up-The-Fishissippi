using UnityEngine;

public class bubble : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField]
    float bounceStrength = 3.0f;

    Rigidbody2D playerRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = gameObject.AddComponent<Rigidbody2D>();
    }

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
