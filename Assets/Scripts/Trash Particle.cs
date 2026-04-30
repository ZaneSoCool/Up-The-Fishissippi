using UnityEngine;

public class TrashParticle : MonoBehaviour
{

    Rigidbody2D playerRB;

    Rigidbody2D trashRB;

    void Start()
    {
        trashRB = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("Player"))
        {
            trashRB.linearVelocity += playerRB.linearVelocity;
        }
    }
}
