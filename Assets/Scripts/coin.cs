using UnityEngine;

public class coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out player playerScript))
        {
            playerScript.coinsCount += 1;
            Destroy(gameObject);
        }
    }
}
