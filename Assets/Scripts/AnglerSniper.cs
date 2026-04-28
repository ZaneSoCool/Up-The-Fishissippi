using System.Collections;
using UnityEngine;

public class AnglerSniper : MonoBehaviour
{
    [Header("Phase Trigger")]
    [SerializeField] private Attackable bossAttackable;
    [SerializeField] private float phaseThreshold = 0.333f;

    [Header("Angler")]
    [SerializeField] private SpriteRenderer anglerRenderer;
    [SerializeField] private Sprite rifleBattleSprite;

    [Header("Rifle")]
    [SerializeField] private Transform rifleTransform;       // child that rotates to aim
    [SerializeField] private SpriteRenderer rifleRenderer;   // hidden until phase 3
    [SerializeField] private Transform rifleTip;             // origin point of laser and bullet
    [SerializeField] private float rifleRotationOffset = 56f;
    [SerializeField] private int lineSortingOrder = -1;      // layer order for laser and bullet.
    [Header("Laser / Bullet")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private float lineExtension = 30f;      // how far past the player the laser/bullet extends
    [SerializeField] private float telegraphDuration = 3f;
    [SerializeField] private float flashIntervalStart = 0.4f;
    [SerializeField] private float flashIntervalEnd = 0.06f;
    [SerializeField] private float bulletSpeed = 18f;
    [SerializeField] private float bulletLength = 0.6f;
    [SerializeField] private float hitRadius = 0.8f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float fireCooldown = 2.5f;

    private static readonly Color laserColor  = Color.red;
    private static readonly Color bulletColor = Color.black;

    private Transform player;
    private Attackable playerAttackable;
    private bool phaseStarted = false;
    private int bossMaxHealth;

    void Start()
    {
        if (bossAttackable != null)
            bossMaxHealth = bossAttackable.CurrentHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerAttackable = playerObj.GetComponent<Attackable>();
        }

        if (rifleRenderer != null) rifleRenderer.enabled = false;

        if (line != null)
        {
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.sortingOrder = lineSortingOrder;
            line.enabled = false;
        }
    }

    void Update()
    {
        if (!TheRoyalFlush.Instance.bossStarted) return;

        if (!phaseStarted && bossAttackable != null)
        {
            if ((float)bossAttackable.CurrentHealth / bossMaxHealth <= phaseThreshold)
                ActivatePhase3();
        }
    }

    void ActivatePhase3()
    {
        phaseStarted = true;

        if (anglerRenderer != null && rifleBattleSprite != null)
            anglerRenderer.sprite = rifleBattleSprite;
        if (rifleRenderer != null) rifleRenderer.enabled = true;

        StartCoroutine(SniperLoop());
    }

    IEnumerator SniperLoop()
    {
        while (true)
        {
            yield return StartCoroutine(TelegraphAndFire());
            yield return new WaitForSeconds(fireCooldown);
        }
    }

    IEnumerator TelegraphAndFire()
    {
        line.startColor = laserColor;
        line.endColor   = laserColor;
        line.enabled = true;

        float elapsed = 0f;
        bool laserVisible = true;
        float toggleCountdown = flashIntervalStart;

        while (elapsed < telegraphDuration)
        {
            AimRifle();
            UpdateLaser();

            elapsed += Time.deltaTime;
            toggleCountdown -= Time.deltaTime;

            if (toggleCountdown <= 0f)
            {
                laserVisible = !laserVisible;
                line.enabled = laserVisible;
                float t = elapsed / telegraphDuration;
                toggleCountdown = Mathf.Lerp(flashIntervalStart, flashIntervalEnd, t);
            }

            yield return null;
        }

        line.enabled = false;
        Vector2 origin = rifleTip.position;
        Vector2 lockedTarget = player != null ? (Vector2)player.position : origin + Vector2.down;
        Vector2 dir = (lockedTarget - origin).normalized;
        Vector2 lockedEndpoint = origin + dir * lineExtension;

        yield return StartCoroutine(FireBullet(lockedTarget, lockedEndpoint));
    }

    IEnumerator FireBullet(Vector2 target, Vector2 endpoint)
    {
        line.startColor = bulletColor;
        line.endColor   = bulletColor;
        line.enabled = true;

        Vector2 origin = rifleTip.position;
        Vector2 dir = (endpoint - origin).normalized;
        float playerDist = Vector2.Distance(origin, target);
        float totalDist  = Vector2.Distance(origin, endpoint);
        float traveled = 0f;
        bool hitDealt = false;

        while (traveled < totalDist + bulletLength)
        {
            traveled += bulletSpeed * Time.deltaTime;

            Vector2 head = origin + dir * Mathf.Min(traveled, totalDist);
            Vector2 tail = origin + dir * Mathf.Max(0f, traveled - bulletLength);
            line.SetPosition(0, tail);
            line.SetPosition(1, head);

            if (!hitDealt && traveled >= playerDist)
            {
                hitDealt = true;
                if (player != null && playerAttackable != null &&
                    Vector2.Distance(player.position, target) < hitRadius)
                {
                    playerAttackable.Attacked(bulletDamage);
                }
            }

            yield return null;
        }

        line.enabled = false;
    }

    void AimRifle()
    {
        if (rifleTransform == null || player == null) return;
        Vector2 dir = (Vector2)player.position - (Vector2)rifleTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rifleRotationOffset;
        rifleTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void UpdateLaser()
    {
        if (line == null || rifleTip == null || player == null) return;
        Vector2 dir = ((Vector2)player.position - (Vector2)rifleTip.position).normalized;
        line.SetPosition(0, rifleTip.position);
        line.SetPosition(1, (Vector2)rifleTip.position + dir * lineExtension);
    }
}
