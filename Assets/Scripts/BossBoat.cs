using UnityEngine;

public class BossBoat : MonoBehaviour
{
    private Attackable attackable;
    private int lastHealth;
    private player playerScript;

    void Start()
    {
        attackable = GetComponent<Attackable>();
        if (attackable != null)
            lastHealth = attackable.CurrentHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerScript = playerObj.GetComponent<player>();
    }

    void Update()
    {
        if (attackable == null || playerScript == null) return;
        if (!TheRoyalFlush.Instance.bossStarted) return;

        if (attackable.CurrentHealth < lastHealth)
        {
            lastHealth = attackable.CurrentHealth;
            playerScript.FlipBashY();
        }
    }
}
