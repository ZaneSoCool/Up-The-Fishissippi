using UnityEngine;

public class GeyserManager : MonoBehaviour
{
    [Header("Pipes")]
    [SerializeField] private Transform pipeA;
    [SerializeField] private Transform pipeB;

    [Header("Trash")]
    [SerializeField] private GameObject trashPrefab;

    [Header("Timing")]
    [SerializeField] private float intervalMin = 2f;
    [SerializeField] private float intervalMax = 4f;

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

        float flightTime = 1f;

        TrashFlow trash = obj.GetComponent<TrashFlow>();
        if (trash != null)
            trash.Initialize(pipeA.position, pipeB.position, flightTime);
    }
}
