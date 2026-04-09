using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField] float bounceStrength = 1.0f;
    int enemyfishDamage = 1;
    Rigidbody2D playerRB;
    private Rigidbody2D rb;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed = 2f;

    Vector2 direction;
    Transform targetPoint;

    [Header("Animation")]
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite swim1Sprite;
    [SerializeField] Sprite swim2Sprite;
    [SerializeField] float frameRate = 8f;

    SpriteRenderer sr;
    Sprite[] frames;
    int frameIndex;
    float frameTimer;

    public void Initialize(Transform a, Transform b)
    {
        pointA = a;
        pointB = b;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (pointA == null || pointB == null)
        {
            Debug.LogError("EnemyFish: pointA or pointB is not assigned.", gameObject);
            enabled = false;
            return;
        }

        targetPoint = pointB;
        direction = (pointB.position - transform.position).normalized;
        UpdateFacing();

        frames = new Sprite[] { idleSprite, swim1Sprite, swim2Sprite };
    }

    void Update()
    {
        if (sr == null || frames == null) return;

        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            frameIndex = (frameIndex + 1) % frames.Length;
            if (frames[frameIndex] != null)
                sr.sprite = frames[frameIndex];
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        if (Vector2.Distance(rb.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointB ? pointA : pointB;
            direction = ((Vector2)targetPoint.position - rb.position).normalized;
            UpdateFacing();
        }
    }

    void UpdateFacing()
    {
        // Flip horizontally based on travel direction (sprite faces left by default)
        if (Mathf.Abs(direction.x) > 0.001f)
            sr.flipX = direction.x > 0;

        // Tilt to match vertical component — formula differs based on which way the sprite is facing
        float angle = sr.flipX
            ? Mathf.Atan2( direction.y,  direction.x) * Mathf.Rad2Deg   // nose at local +X when flipped
            : Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;  // nose at local -X when not flipped
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 bounceDir = ((Vector2)collision.transform.position - rb.position).normalized;
            playerRB.linearVelocity = bounceDir * bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(enemyfishDamage);
        }
    }
}
