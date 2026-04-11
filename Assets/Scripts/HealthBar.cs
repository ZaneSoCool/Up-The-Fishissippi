using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartLost;

    private Image[] heartImages;
    private Attackable playerAttackable;

    void Start()
    {
        // Auto-find heart Images from children (order matches sibling index)
        heartImages = GetComponentsInChildren<Image>(includeInactive: true);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerAttackable = player.GetComponent<Attackable>();
    }

    void Update()
    {
        if (playerAttackable == null || heartImages == null) return;
        SetHearts(playerAttackable.CurrentHealth);
    }

    void SetHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = (i < currentHealth) ? heartFull : heartLost;
        }
    }
}
