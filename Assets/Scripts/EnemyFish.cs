using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 1.0f;

    [SerializeField]
    int enemyfishDamage = 1;

    Rigidbody2D playerRB;

    private Rigidbody2D rb;

    private float currentSpeed;

    [SerializeField]
    float speed = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = rb.linearVelocity.magnitude * speed;
        Debug.Log(currentSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB.linearVelocity *= -bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(enemyfishDamage);
        }

    }
}
