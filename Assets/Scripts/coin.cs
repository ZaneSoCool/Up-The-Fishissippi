using UnityEngine;
using UnityEngine.UI;

public class coin : MonoBehaviour
{
    public int value;

    [SerializeField] AudioClip coincollectClip;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out player playerScript))
        {
            playerScript.coinsCount += value;
            if (coincollectClip != null) AudioSource.PlayClipAtPoint(coincollectClip, transform.position);
            Destroy(gameObject);
        }
    }
}
