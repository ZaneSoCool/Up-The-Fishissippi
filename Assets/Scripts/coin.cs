using UnityEngine;
using UnityEngine.UI;

public class coin : MonoBehaviour
{
    public int value;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out player playerScript))
        {
            playerScript.coinsCount += value;
            Destroy(gameObject);
        }
    }
}
