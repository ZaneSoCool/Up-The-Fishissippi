using System.Collections;
using UnityEngine;

public class Bobby : MonoBehaviour
{
    [Header("Phase Trigger")]
    [SerializeField] private Attackable bossAttackable;
    [SerializeField] private float phaseThreshold = 0.667f; // activates at 2/3 boss HP

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float turnSpeed = 60f; // degrees per second: lower = clunkier
    [SerializeField] private float aboveWaterGravity = 10f;
    [SerializeField] private float maxFallSpeed = 8f;

    [Header("Knockback")]
    [SerializeField] private float knockbackSpeed = 5f;
    [SerializeField] private float knockbackDuration = 0.4f;
    [SerializeField] private float hitStunDuration = 0.3f;
    [SerializeField] private float bashBounceSpeed = 3f;

    [Header("Damage")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float bounceStrength = 3.0f;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Attackable attackable;
    private Transform player;
    private Attackable playerAttackable;
    private player playerScript;

    private int bossMaxHealth;
    private int lastHealth;

    private bool phaseStarted = false;
    private bool isLaunching = false;
    private bool isKnockedBack = false;
    private bool isHitStunned = false;
    private float damageTimer = 0f;
    private float fallVelocity = 0f;
    private float currentAngle = 0f;

    private Vector2 knockbackDir;
    private float knockbackTimer = 0f;
    private Rigidbody2D playerRB;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attackable = GetComponent<Attackable>();

        if (bossAttackable != null)
            bossMaxHealth = bossAttackable.CurrentHealth;
        if (attackable != null)
            lastHealth = attackable.CurrentHealth;

        anim.enabled = false; // stay on idle sprite while on the boat

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerAttackable = playerObj.GetComponent<Attackable>();
            playerScript = playerObj.GetComponent<player>();
        }
    }

    void Update()
    {
        if (!TheRoyalFlush.Instance.bossStarted) return;

        damageTimer -= Time.deltaTime;

        // Watch for phase 2 trigger
        if (!phaseStarted && bossAttackable != null)
        {
            if ((float)bossAttackable.CurrentHealth / bossMaxHealth <= phaseThreshold)
                ActivatePhase2();
        }

        if (!phaseStarted) return;
        if (isLaunching) return;

        // Skull bash hit detected via health drop
        if (attackable != null && attackable.CurrentHealth < lastHealth)
        {
            lastHealth = attackable.CurrentHealth;
            StartKnockback();
        }

        float waterY = TheRoyalFlush.Instance.WaterLevel;

        // Above water: fall down (no swimming yet)
        if (transform.position.y > waterY)
        {
            fallVelocity = Mathf.Min(fallVelocity + aboveWaterGravity * Time.deltaTime, maxFallSpeed);
            transform.position += Vector3.down * fallVelocity * Time.deltaTime;
            return;
        }

        fallVelocity = 0f;

        // Slowly turn to face the player — Bobby's head points UP in the sprite so offset by -90°
        if (player != null)
        {
            Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
            float targetAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }

        // Knockback
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            transform.position += (Vector3)(knockbackDir * knockbackSpeed * Time.deltaTime);
            if (knockbackTimer <= 0f) isKnockedBack = false;
            return;
        }

        if (isHitStunned || player == null) return;

        // Move in the direction Bobby is currently facing (his head = transform.up)
        transform.position += transform.up * chaseSpeed * Time.deltaTime;
    }

    void ActivatePhase2()
    {
        phaseStarted = true;
        transform.SetParent(null); // detach so boat movement doesn't carry Bobby
        anim.enabled = true;
        StartCoroutine(DiveSequence());
    }

    IEnumerator DiveSequence()
    {
        isLaunching = true;

        currentAngle = -180f;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        // Small hop off the boat deck
        float elapsed = 0f;
        float hopDuration = 0.25f;
        float hopSpeed = 4f;
        while (elapsed < hopDuration)
        {
            elapsed += Time.deltaTime;
            float vy = Mathf.Lerp(hopSpeed, 0f, elapsed / hopDuration);
            transform.position += Vector3.up * vy * Time.deltaTime;
            yield return null;
        }

        // Fall until Bobby crosses the water surface
        fallVelocity = 0f;
        while (transform.position.y > TheRoyalFlush.Instance.WaterLevel)
        {
            fallVelocity = Mathf.Min(fallVelocity + aboveWaterGravity * Time.deltaTime, maxFallSpeed);
            transform.position += Vector3.down * fallVelocity * Time.deltaTime;
            yield return null;
        }

        fallVelocity = 0f;
        isLaunching = false;
    }

    void StartKnockback()
    {
        knockbackDir = player != null
            ? ((Vector2)transform.position - (Vector2)player.position).normalized
            : Vector2.down;

        isKnockedBack = true;
        isHitStunned = true;
        knockbackTimer = knockbackDuration;

        if (playerScript != null)
        {
            Vector2 bounceDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            playerScript.BashBounce(bounceDir, bashBounceSpeed);
        }

        StartCoroutine(HitStunRoutine());
    }

    IEnumerator HitStunRoutine()
    {
        yield return new WaitForSeconds(knockbackDuration + hitStunDuration);
        isHitStunned = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || damageTimer > 0f) return;
        if (playerAttackable != null) playerAttackable.Attacked(contactDamage);
        damageTimer = damageCooldown;
        playerRB = col.gameObject.GetComponent<Rigidbody2D>();
        playerRB.linearVelocity *= -bounceStrength;       
    }
}
