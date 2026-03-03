using UnityEngine;

public class urchin : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField]
    float bounceStrength = 1.0f;
    
    Rigidbody2D playerRB;

    [SerializeField]
    player gillgamesh;


<<<<<<< Updated upstream
    // Start is called once before the fir
}
=======
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = gameObject.AddComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
            gillgamesh.playerHealth -= 1;
            Debug.Log("Player Health = " + gillgamesh.playerHealth);
        }

    }
}
>>>>>>> Stashed changes
