using UnityEngine;

public class bubble : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField]
    float bounceStrength = 3.0f;

    [SerializeField]
    Rigidbody2D playerRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = gameObject.AddComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called when the cube hits the floor
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
        }
    }
}
