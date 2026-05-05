using UnityEngine;

public class BossBoat : MonoBehaviour
{
    [SerializeField] private float bashBounceSpeed = 3f;

    private Attackable attackable;
    private int lastHealth;
    private int maxHealth;
    private player playerScript;

    private bool phase2Triggered = false;
    private bool phase3Triggered = false;
    private bool deathTriggered = false;

    void Start()
    {
        attackable = GetComponent<Attackable>();
        if (attackable != null)
        {
            maxHealth = attackable.CurrentHealth;
            lastHealth = maxHealth;
            attackable.onDeath = () =>
            {
                if (deathTriggered) return;
                deathTriggered = true;
                TheRoyalFlush.Instance?.OnBossDefeated();
            };
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerScript = playerObj.GetComponent<player>();
    }

    void Update()
    {
        if (attackable == null || playerScript == null) return;
        if (TheRoyalFlush.Instance == null || !TheRoyalFlush.Instance.bossStarted) return;
        if (deathTriggered) return;

        int currentHealth = attackable.CurrentHealth;

        if (currentHealth < lastHealth)
        {
            lastHealth = currentHealth;
            Vector2 bounceDir = ((Vector2)playerScript.transform.position - (Vector2)transform.position).normalized;
            playerScript.BashBounce(bounceDir, bashBounceSpeed);
            TheRoyalFlush.Instance?.OnBoatHit();
        }

        if (maxHealth > 0)
        {
            float fraction = (float)currentHealth / maxHealth;
            if (!phase2Triggered && fraction <= 2f / 3f)
            {
                phase2Triggered = true;
                TheRoyalFlush.Instance?.StartPhase2();
            }
            if (!phase3Triggered && fraction <= 1f / 3f)
            {
                phase3Triggered = true;
                TheRoyalFlush.Instance?.StartPhase3();
            }
        }
    }
}
