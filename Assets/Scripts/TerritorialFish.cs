using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState { Patrol, Alert, Chase }

public class TerritorialFish : Territory
{
    [Header("Detection Distances")]
    public float alertDistance = 4f;
    public float chaseDistance = 2f;

    [Header("Movement Speeds")]
    public float alertSpeed = 1.5f;
    public float chaseSpeed = 4f;

    [Header("Territory Settings")]
    public float territoryRadius = 6f;

    [Header("Patrol Settings")]
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;
    public bool clockwise = true;

    private Rigidbody2D rb;

    private Rigidbody2D playerRB;

    private float bounceStrength = 1f;

    public int walleyeDamage = 1;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private CircleCollider2D territoryCollider;
    private EnemyState currentState = EnemyState.Patrol;

    private float patrolAngle = 0f;
    private Vector2 patrolCenter;

    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        territoryCollider = GetComponent<CircleCollider2D>();
        if (territoryCollider == null)
            territoryCollider = gameObject.AddComponent<CircleCollider2D>();

        territoryCollider.isTrigger = true;
        territoryCollider.radius = territoryRadius;
    }

    private void Start()
    {
        patrolCenter = transform.position;
    }

    private void Update()
    {
        if (currentState == EnemyState.Patrol)
        {
            Patrol();
        }
    }

    private void SetupTerritoryTrigger()
    {
        GameObject territoryObject = new GameObject("TerritoryTrigger");
        territoryObject.transform.SetParent(transform);
        territoryObject.transform.localPosition = Vector3.zero;

        CircleCollider2D territoryCollider = territoryObject.AddComponent<CircleCollider2D>();
        territoryCollider.isTrigger = true;
        territoryCollider.radius = territoryRadius;

        TerritoryTriggerRelay relay = territoryObject.AddComponent<TerritoryTriggerRelay>();
        relay.Initialize(this);

        // Assign Territory layer to child so it doesn't cause damage collisions
        int territoryLayer = LayerMask.NameToLayer("Territory");
        if (territoryLayer != -1)
            territoryObject.layer = territoryLayer;
        else
            Debug.LogWarning("Territory layer not found! Please add it in Project Settings.");
    }


    private void Patrol()
    {
        float direction = clockwise ? -1f : 1f;
        patrolAngle += direction * patrolSpeed * Time.deltaTime;

        if (patrolAngle > 360f) patrolAngle -= 360f;
        if (patrolAngle < 0f) patrolAngle += 360f;

        float x = patrolCenter.x + Mathf.Cos(patrolAngle) * patrolRadius;
        float y = patrolCenter.y + Mathf.Sin(patrolAngle) * patrolRadius;
        Vector2 targetPosition = new Vector2(x, y);

        rb.MovePosition(targetPosition);

        // Flip sprite based on movement direction
        Vector2 moveDirection = targetPosition - rb.position;
        spriteRenderer.flipX = moveDirection.x < 0;
    }
    public void PlayerEnteredTerritory(Transform player)
    {
        OnPlayerEnterTerritory(player);
    }

    // Called by TerritoryTriggerRelay when player exits territory
    public void PlayerExitedTerritory(Transform player)
    {
        OnPlayerExitTerritory(player);
    }

    protected override void OnPlayerEnterTerritory(Transform player)
    {
        base.OnPlayerEnterTerritory(player);
        animator.SetTrigger("Chase");
        // flip enemy sprite to face player, play alert sound
        Vector2 directionToPlayer = player.position - transform.position;
        spriteRenderer.flipX = directionToPlayer.x < 0;

        audioSource.Play();
        SetState(EnemyState.Alert);
    }

    protected override void OnPlayerExitTerritory(Transform player)
    {
        base.OnPlayerExitTerritory(player);
        // return enemy to patrol state
        SetState(EnemyState.Patrol);
    }

    protected override void OnPlayerMoved(Transform player, Vector2 localPos, float distance)
    {
        float distanceToPlayer = Vector2.Distance(rb.position, player.position);

        if (distanceToPlayer < chaseDistance)
        {
            Debug.Log($"Player too close! Chasing! Distance: {distance:F2}");
            // e.g., trigger chase behavior, deal damage, etc.
            SetState(EnemyState.Chase);
            Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * chaseSpeed * Time.deltaTime);
        }
        else if (distance < alertDistance)
        {
            Debug.Log($"Player detected! Alerting! Distance: {distance:F2}");
            // e.g., move toward player slowly
            SetState(EnemyState.Alert);
            Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * alertSpeed * Time.deltaTime);
        }
        else
        {
            SetState(EnemyState.Patrol);
        }
        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore territory trigger collisions
        if (collision.gameObject.layer == LayerMask.NameToLayer("Territory")) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRB != null)
            {
                playerRB.linearVelocity *= -bounceStrength;

                Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
                if (playerAttackableScript != null)
                    playerAttackableScript.Attacked(walleyeDamage);
            }
        }
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        Debug.Log($"[{gameObject.name}] State changed to: {newState}");
    }

    private void WalleyeChase()
    {
        Debug.Log("Walleye chasing Player");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the patrol circle in the editor
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ?
            new Vector3(patrolCenter.x, patrolCenter.y, 0f) : transform.position;

        int segments = 36;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(patrolRadius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * patrolRadius,
                Mathf.Sin(angle) * patrolRadius, 0f);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    private void DrawGizmoCircle(Vector3 center, float radius, int segments = 36)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius, 0f);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
