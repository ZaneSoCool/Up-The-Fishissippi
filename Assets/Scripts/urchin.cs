using UnityEngine;

public class urchin : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 1.0f;

    [SerializeField]
    int urchinDamage = 1;
    
    Rigidbody2D playerRB;

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(urchinDamage);
        }
    }
}
