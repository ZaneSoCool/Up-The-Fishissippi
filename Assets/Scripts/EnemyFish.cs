using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 1.0f;

    int enemyfishDamage = 1;

    Rigidbody2D playerRB;

    private Rigidbody2D rb;

    [SerializeField]
    Transform pointA;

    [SerializeField]
    Transform pointB;

    [SerializeField]
    float speed = 1.5f;

    Vector2 direction;
    Transform targetPoint;

    //DistanceTraveledTracker distanceTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        targetPoint = pointB;
        direction = (pointB.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        if (Vector2.Distance(rb.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointB ? pointA : pointB;
            direction = ((Vector2)targetPoint.position - rb.position).normalized;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
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
