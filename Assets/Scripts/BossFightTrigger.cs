using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{

    [SerializeField] private float triggerTimer = 0f;
    [SerializeField] private float triggerDuration = 1f;
    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        triggerTimer += Time.deltaTime;
        if (triggerTimer >= triggerDuration)
        {
            TheRoyalFlush.Instance?.StartBossFight();
            gameObject.SetActive(false);
        }
    }
}
