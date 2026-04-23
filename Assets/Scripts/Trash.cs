using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 1.0f;

    [SerializeField]
    int trashDamage = 1;

    Rigidbody2D playerRB;

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Trash");
            playerRB.linearVelocity *= -bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(trashDamage);
        }
    }
}
