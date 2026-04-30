using System.Collections;
using UnityEngine;

public class TheRoyalFlush : MonoBehaviour
{
    public static TheRoyalFlush Instance { get; private set; }

    [Header("Battle State")]
    public bool bossStarted = false;
    [SerializeField] private BossHealthBar healthBar;

    [Header("Water")]
    [SerializeField] private Transform boat;
    public float WaterLevel => boat.position.y;

    [Header("Character Sprite Renderers")]
    [SerializeField] private SpriteRenderer mongerRenderer;
    [SerializeField] private SpriteRenderer anglerRenderer;
    [SerializeField] private SpriteRenderer bobbyRenderer;

    [Header("Battle Idle Sprites")]
    [SerializeField] private Sprite mongerBattleSprite;
    [SerializeField] private Sprite anglerBattleSprite;
    [SerializeField] private Sprite bobbyBattleSprite;

    [Header("Hit Sprites")]
    [SerializeField] private Sprite mongerHitSprite;
    [SerializeField] private Sprite bobbyHitSprite;
    [SerializeField] private float hitFlashDuration = 0.5f;
    public bool bobbyOnBoat = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        StartBossFight();
    }

    public void StartBossFight()
    {
        if (bossStarted) return;
        bossStarted = true;

        if (healthBar != null) healthBar.gameObject.SetActive(true);

        if (mongerRenderer != null && mongerBattleSprite != null)
            mongerRenderer.sprite = mongerBattleSprite;
        if (anglerRenderer != null && anglerBattleSprite != null)
            anglerRenderer.sprite = anglerBattleSprite;
        if (bobbyRenderer != null && bobbyBattleSprite != null)
            bobbyRenderer.sprite = bobbyBattleSprite;
    }

    public void OnBoatHit()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        if (mongerRenderer != null && mongerHitSprite != null)
            mongerRenderer.sprite = mongerHitSprite;
        if (bobbyOnBoat && bobbyRenderer != null && bobbyHitSprite != null)
            bobbyRenderer.sprite = bobbyHitSprite;

        yield return new WaitForSeconds(hitFlashDuration);

        if (mongerRenderer != null && mongerBattleSprite != null)
            mongerRenderer.sprite = mongerBattleSprite;
        if (bobbyOnBoat && bobbyRenderer != null && bobbyBattleSprite != null)
            bobbyRenderer.sprite = bobbyBattleSprite;
    }
}
