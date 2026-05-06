using UnityEngine;

public class GeyserManager : MonoBehaviour
{
    [Header("Pipes")]
    [SerializeField] private Transform pipeA;          // spawn point (left pipe)
    [SerializeField] private Transform pipeB;          // landing point (right pipe)

    [Header("Trash")]
    [SerializeField] private GameObject trashPrefab;

    [Header("Timing")]
    [SerializeField] private float intervalMin = 4f;
    [SerializeField] private float intervalMax = 8f;

    private float _timer;

    private void Start()
    {
        _timer = Random.Range(intervalMin, intervalMax);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnFlow();
            _timer = Random.Range(intervalMin, intervalMax);
        }
    }

    private void SpawnFlow()
    {
        if (trashPrefab == null) return;

        GameObject obj = Instantiate(trashPrefab, pipeA.position, Quaternion.identity);

        float flightTime = Random.Range(0.6f, 1.2f);

        TrashFlow trash = obj.GetComponent<TrashFlow>();
        if (trash != null)
            trash.Initialize(pipeA.position, pipeB.position, flightTime);
    }
}
