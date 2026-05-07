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
    [SerializeField] private GameObject mongerHook;

    [Header("Battle Idle Sprites")]
    [SerializeField] private Sprite mongerBattleSprite;
    [SerializeField] private Sprite anglerBattleSprite;
    [SerializeField] private Sprite bobbyBattleSprite;

    [Header("Hit Sprites")]
    [SerializeField] private Sprite mongerHitSprite;
    [SerializeField] private Sprite bobbyHitSprite;
    [SerializeField] private float hitFlashDuration = 0.5f;
    public bool bobbyOnBoat = true;

    [Header("Phase Scripts")]
    [SerializeField] private Bobby bobby;
    [SerializeField] private AnglerSniper anglerSniper;
    [SerializeField] private BossBoat bossBoat;
    [SerializeField] private Attackable bossAttackable;
    [SerializeField] private Boat boatMovement;
    [SerializeField] private Transform bobbyReturnPoint;

    [Header("Cutscenes")]
    [SerializeField] private CutsceneDirector.DialogLine[] introLines;
    [SerializeField] private CutsceneDirector.DialogLine[] phase2Lines;
    [SerializeField] private CutsceneDirector.DialogLine[] phase3Lines;
    [SerializeField] private CutsceneDirector.DialogLine[] outroLines;

    [Header("Defeat Sprites")]
    [SerializeField] private Sprite mongerDefeatSprite;
    [SerializeField] private Sprite anglerDefeatSprite;
    [SerializeField] private Sprite bobbyDefeatSprite;

    [Header("Defeat Sequence")]
    [SerializeField] private Animator boatAnimator;
    [SerializeField] private string defeatStateName = "Defeat";
    [SerializeField] private string explosionStateName = "Explosion";
    [SerializeField] private float defeatFadeDuration = 1.5f;

    [Header("End Screen")]
    [SerializeField] private GameObject toBeContinuedPanel;
    [SerializeField] private float endScreenFadeIn = 1f;

    [Header("Boat")]
    [SerializeField] private SpriteRenderer boatSpriteRenderer;

    private CutsceneDirector _cutsceneDirector;
    private bool _introStarted = false;

    private Color _originalColor = Color.white;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _cutsceneDirector = GetComponent<CutsceneDirector>();
        if (mongerHook != null) mongerHook.SetActive(false);
    }

    void Start()
    {

        healthBar = PersistentRoot.Instance?.GetComponentInChildren<BossHealthBar>(true);
        if (healthBar != null) healthBar.gameObject.SetActive(false);
    }

    public void StartBossFight()
    {
        if (_introStarted) return;
        _introStarted = true;

        if (roomExit != null) roomExit.Lock();

        if (_cutsceneDirector != null)
            _cutsceneDirector.Play(introLines, OnIntroComplete);
        else
            OnIntroComplete();
    }

    private void OnIntroComplete()
    {
        bossStarted = true;
        if (boatMovement != null) boatMovement.movementDisabled = false;
        MusicManager.Instance?.StopBGMusic();

        if (healthBar != null)
        {
            if (bossAttackable != null) healthBar.Initialize(bossAttackable);
            healthBar.gameObject.SetActive(true);
        }
        if (mongerHook != null) mongerHook.SetActive(true);

        if (mongerRenderer != null && mongerBattleSprite != null)
            mongerRenderer.sprite = mongerBattleSprite;
        if (anglerRenderer != null && anglerBattleSprite != null)
            anglerRenderer.sprite = anglerBattleSprite;
        if (bobbyRenderer != null && bobbyBattleSprite != null)
            bobbyRenderer.sprite = bobbyBattleSprite;
    }

    public void StartPhase2()
    {
        boatSpriteRenderer.material.color = _originalColor;
        _cutsceneDirector.Play(phase2Lines, () => bobby?.ActivatePhase2());
    }

    public void StartPhase3()
    {
        boatSpriteRenderer.material.color = _originalColor;
        _cutsceneDirector.Play(phase3Lines, () => anglerSniper?.ActivatePhase3());
    }

    // Call this when the boss is defeated
    public void OnBossDefeated()
    {
        boatSpriteRenderer.material.color = _originalColor;
        Time.timeScale = 0f;
        if (bossBoat != null) bossBoat.movementDisabled = true;
        if (boatMovement != null) boatMovement.movementDisabled = true;
        if (boatAnimator != null) boatAnimator.Play(defeatStateName);

        if (bobby != null && bobbyReturnPoint != null)
        {
            bobbyOnBoat = true;
            bobby.ReturnToBoat(boat, bobbyReturnPoint.position, OnBobbyReturned);
        }
        else
        {
            OnBobbyReturned();
        }
    }

    private void OnBobbyReturned()
    {
        if (mongerRenderer != null && mongerDefeatSprite != null)
            mongerRenderer.sprite = mongerDefeatSprite;
        if (anglerRenderer != null && anglerDefeatSprite != null)
            anglerRenderer.sprite = anglerDefeatSprite;
        if (bobbyRenderer != null && bobbyDefeatSprite != null)
            bobbyRenderer.sprite = bobbyDefeatSprite;

        _cutsceneDirector.Play(outroLines, () => StartCoroutine(DefeatSequence()));
    }

    private IEnumerator DefeatSequence()
    {
        if (boatAnimator != null)
        {
            boatAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            boatAnimator.Play(explosionStateName);
            yield return null; // wait one frame for the state to start
            while (true)
            {
                AnimatorStateInfo info = boatAnimator.GetCurrentAnimatorStateInfo(0);
                if (info.IsName(explosionStateName) && info.normalizedTime >= 1f) break;
                yield return null;
            }
        }
        
        yield return StartCoroutine(RoomTransitionManager.Instance.FadeToBlack(defeatFadeDuration));

        if (toBeContinuedPanel != null)
        {
            toBeContinuedPanel.SetActive(true);
            CanvasGroup cg = toBeContinuedPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                float t = 0f;
                while (t < endScreenFadeIn)
                {
                    t += Time.unscaledDeltaTime;
                    cg.alpha = Mathf.Clamp01(t / endScreenFadeIn);
                    yield return null;
                }
                cg.alpha = 1f;
            }
        }
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
