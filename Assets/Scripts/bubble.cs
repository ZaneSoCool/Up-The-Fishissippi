using UnityEngine;

public class bubble : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField]
    float bounceStrength = 50.0f;

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
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //bounce player
            if (playerRB.linearVelocityX > playerRB.linearVelocityY){
                playerRB.linearVelocityX = -playerRB.linearVelocityX * bounceStrength;
            } else
            {
                playerRB.linearVelocityY = -playerRB.linearVelocityY * bounceStrength;
            }
            //playerRB.linearVelocity = new Vector2(-playerRB.linearVelocityX * bounceStrength, 0);
        }
    }
}
