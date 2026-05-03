using System.Collections;
using UnityEngine;

public class TheRoyalFlush : MonoBehaviour
{
    public static TheRoyalFlush Instance { get; private set; }

    [Header("Battle State")]
    public bool bossStarted = false;
    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private ExitTrigger roomExit;

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

    [Header("Cutscenes")]
    [SerializeField] private CutsceneDirector.DialogLine[] introLines;
    [SerializeField] private CutsceneDirector.DialogLine[] phase2Lines;
    [SerializeField] private CutsceneDirector.DialogLine[] phase3Lines;
    [SerializeField] private CutsceneDirector.DialogLine[] outroLines;

    private CutsceneDirector _cutsceneDirector;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _cutsceneDirector = GetComponent<CutsceneDirector>();
    }

    void Start()
    {
        StartBossFight();
    }

    public void StartBossFight()
    {
        if (bossStarted) return;
        bossStarted = true;

        if (roomExit != null) roomExit.Lock();

        // Play intro cutscene, then activate fight mechanics once it ends
        _cutsceneDirector.Play(introLines, OnIntroComplete);
    }

    private void OnIntroComplete()
    {
        if (healthBar != null) healthBar.gameObject.SetActive(true);

        if (mongerRenderer != null && mongerBattleSprite != null)
            mongerRenderer.sprite = mongerBattleSprite;
        if (anglerRenderer != null && anglerBattleSprite != null)
            anglerRenderer.sprite = anglerBattleSprite;
        if (bobbyRenderer != null && bobbyBattleSprite != null)
            bobbyRenderer.sprite = bobbyBattleSprite;
    }

    // Call this when the boss enters phase 2
    public void StartPhase2()
    {
        _cutsceneDirector.Play(phase2Lines);
    }
    public void StartPhase3()
    {
        _cutsceneDirector.Play(phase3Lines);
    }

    // Call this when the boss is defeated
    public void OnBossDefeated()
    {
        _cutsceneDirector.Play(outroLines, () =>
        {
            if (roomExit != null) roomExit.Unlock();
        });
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
