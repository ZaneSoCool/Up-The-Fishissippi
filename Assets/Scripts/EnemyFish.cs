using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 1.0f;

    [SerializeField]
    int enemyfishDamage = 1;

    Rigidbody2D playerRB;

    private Rigidbody2D rb;

    [SerializeField]
    float speed = 0.2f;

    float reverse = -1.0f;

    Vector2 direction;

    DistanceTraveledTracker distanceTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        distanceTracker = GetComponent<DistanceTraveledTracker>();
        direction = Vector2.down;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        if (distanceTracker.TotalDistance >= 8.0f)
        {
            direction *= reverse;
            distanceTracker.ResetDistance();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity = direction * -bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(enemyfishDamage);
        }

    }
}
