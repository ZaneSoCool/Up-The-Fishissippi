using System.Collections;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider != null) _collider.enabled = false;
        StartCoroutine(EnableAfterTransition());
    }

    private IEnumerator EnableAfterTransition()
    {
        yield return new WaitUntil(() =>
            RoomTransitionManager.Instance == null ||
            !RoomTransitionManager.Instance.IsTransitioning);
        if (_collider != null) _collider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        TheRoyalFlush.Instance?.StartBossFight();
        gameObject.SetActive(false);
    }
}
