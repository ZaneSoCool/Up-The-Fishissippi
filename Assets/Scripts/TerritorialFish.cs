// Written by Nathan Gumagay in collaboration with Claude
using UnityEngine;

public enum EnemyState { Patrol, Alert }

public class TerritorialFish : Territory
{
    [Header("Detection Distance")]
    public float alertDistance = 4f;

    [Header("Movement Speeds")]
    public float alertSpeed = 20f;

    [Header("Territory Settings")]
    public float territoryRadius = 6f;

    [Header("Patrol Settings")]
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;
    public bool clockwise = true;

    private Rigidbody2D rb;
    private Rigidbody2D playerRB;
    private float bounceStrength = 15f;
    public int walleyeDamage = 1;
    private SpriteRenderer spriteRenderer;
    //private AudioSource audioSource;
    private EnemyState currentState = EnemyState.Patrol;

    private Transform trackedPlayer;

    private float patrolAngle = 0f;
    private Vector2 patrolCenter;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        SetupBodyCollider();
        SetupTerritoryTrigger();
    }

    private void Start()
    {
        patrolCenter = transform.position;
    }

    private void Update()
    {
        if (currentState == EnemyState.Patrol)
            Patrol();
        else if (currentState == EnemyState.Alert && trackedPlayer != null)
            ChasePlayer();
    }

    private void SetupBodyCollider()
    {
        GameObject bodyObject = new GameObject("BodyCollider");
        bodyObject.transform.SetParent(transform);
        bodyObject.transform.localPosition = Vector3.zero;

        CircleCollider2D bodyCollider = bodyObject.AddComponent<CircleCollider2D>();
        bodyCollider.isTrigger = true;
        bodyCollider.radius = 0.5f;

        EnemyBodyCollider bodyScript = bodyObject.AddComponent<EnemyBodyCollider>();
        bodyScript.Initialize(this);

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (enemyLayer != -1)
            bodyObject.layer = enemyLayer;
        else
            Debug.LogWarning("Enemy layer not found!");
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

        int territoryLayer = LayerMask.NameToLayer("Territory");
        if (territoryLayer != -1)
            territoryObject.layer = territoryLayer;
        else
            Debug.LogWarning("Territory layer not found!");
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

        // Calculate the tangent direction based on clockwise or counterclockwise
        // This gives the actual forward direction of movement around the circle
        Vector2 tangentDirection = clockwise
            ? new Vector2(Mathf.Sin(patrolAngle), -Mathf.Cos(patrolAngle))   // Clockwise tangent
            : new Vector2(-Mathf.Sin(patrolAngle), Mathf.Cos(patrolAngle));  // Counterclockwise tangent

        float angle = Mathf.Atan2(tangentDirection.y, tangentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ChasePlayer()
    {

        Vector2 directionToPlayer = ((Vector2)trackedPlayer.position - rb.position).normalized;
        rb.MovePosition(rb.position + directionToPlayer * alertSpeed * Time.deltaTime);
        spriteRenderer.flipX = trackedPlayer.position.x < transform.position.x;

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void PlayerEnteredTerritory(Transform player)
    {
        OnPlayerEnterTerritory(player);
    }

    public void PlayerExitedTerritory(Transform player)
    {
        OnPlayerExitTerritory(player);
    }

    protected override void OnPlayerEnterTerritory(Transform player)
    {
        base.OnPlayerEnterTerritory(player);
        trackedPlayer = player;
        animator.SetTrigger("Chase");
        //audioSource.Play();
        SetState(EnemyState.Alert);
    }

    protected override void OnPlayerExitTerritory(Transform player)
    {
        base.OnPlayerExitTerritory(player);
        trackedPlayer = null;
        SetState(EnemyState.Patrol);
    }

    protected override void OnPlayerMoved(Transform player, Vector2 localPos, float distance)
    {

        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    public void HandlePlayerBodyContact(Collider2D collision)
    {
        playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

        if (playerRB != null)
        {
            // Calculate direction from fish to player
            Vector2 bounceDirection = ((Vector2)collision.transform.position - rb.position).normalized;

            // Apply bounce force regardless of player's current velocity
            playerRB.linearVelocity = bounceDirection * bounceStrength;

            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            if (playerAttackableScript != null)
                playerAttackableScript.Attacked(walleyeDamage);
        }
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        Debug.Log($"[{gameObject.name}] State changed to: {newState}");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying ?
            new Vector3(patrolCenter.x, patrolCenter.y, 0f) : transform.position;

        // Yellow = patrol circle
        Gizmos.color = Color.yellow;
        DrawGizmoCircle(center, patrolRadius);

        // Cyan = alert distance
        Gizmos.color = Color.cyan;
        DrawGizmoCircle(transform.position, alertDistance);

        // White = territory zone
        Gizmos.color = Color.white;
        DrawGizmoCircle(center, territoryRadius);
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