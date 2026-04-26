using UnityEngine;

public class TheRoyalFlush : MonoBehaviour
{
    public static TheRoyalFlush Instance { get; private set; }

    [Header("Battle State")]
    public bool bossStarted = false;

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

        if (mongerRenderer != null && mongerBattleSprite != null)
            mongerRenderer.sprite = mongerBattleSprite;
        if (anglerRenderer != null && anglerBattleSprite != null)
            anglerRenderer.sprite = anglerBattleSprite;
        if (bobbyRenderer != null && bobbyBattleSprite != null)
            bobbyRenderer.sprite = bobbyBattleSprite;
    }
}
