using UnityEngine;

public class MongerHook : MonoBehaviour
{
    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float returnSpeed = 1.5f;

    [Header("Bounce")]
    [SerializeField] private float bounceSpeed = 8f;
    [SerializeField] private float bounceDuration = 0.5f;

    [Header("Damage")]
    [SerializeField] private int hookDamage = 1;
    [SerializeField] private float damageCooldown = 1f;

    [Header("Line")]
    [SerializeField] private Transform poletip;
    [SerializeField] private LineRenderer lineRenderer;

    private Transform player;
    private Attackable playerAttackable;
    private Attackable attackable;
    private int lastHealth;

    private bool isBouncing = false;
    private float bounceTimer = 0f;
    private Vector2 bounceDirection;
    private float damageTimer = 0f;

    void Start()
    {
        // Detach from the boat so the boat's movement doesn't carry the hook along.
        // The LineRenderer uses world-space positions, so the visual line to poletip still works.
        transform.SetParent(null);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerAttackable = playerObj.GetComponent<Attackable>();
        }

        attackable = GetComponent<Attackable>();
        if (attackable != null)
            lastHealth = attackable.CurrentHealth;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }
    }

    void Update()
    {
        if (!TheRoyalFlush.Instance.bossStarted) return;
        if (player == null) return;

        damageTimer -= Time.deltaTime;

        // Detect a tail thwap hit by watching for a health decrease on our Attackable component.
        if (attackable != null && attackable.CurrentHealth < lastHealth)
        {
            lastHealth = attackable.CurrentHealth;
            StartBounce((Vector2)(transform.position - player.position));
        }

        if (isBouncing)
        {
            bounceTimer -= Time.deltaTime;
            transform.position += (Vector3)(bounceDirection * bounceSpeed * Time.deltaTime);
            if (bounceTimer <= 0f) isBouncing = false;
        }
        else
        {
            // Detection is measured from the pole tip (on the boat), not from the hook itself.
            float distToPlayer = Vector2.Distance(poletip.position, player.position);
            if (distToPlayer < detectionRange)
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, poletip.position, returnSpeed * Time.deltaTime);
        }

        // Keep the hook at or below the water surface.
        float waterY = TheRoyalFlush.Instance.WaterLevel;
        if (transform.position.y > waterY)
            transform.position = new Vector2(transform.position.x, waterY);

        UpdateLine();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (damageTimer > 0f) return;

        if (playerAttackable != null)
            playerAttackable.Attacked(hookDamage);

        damageTimer = damageCooldown;
        StartBounce((Vector2)(transform.position - player.position));
    }

    void StartBounce(Vector2 direction)
    {
        bounceDirection = direction.normalized;
        bounceTimer = bounceDuration;
        isBouncing = true;
    }

    void UpdateLine()
    {
        if (lineRenderer == null || poletip == null) return;
        lineRenderer.SetPosition(0, poletip.position);
        lineRenderer.SetPosition(1, transform.position);
    }
}
