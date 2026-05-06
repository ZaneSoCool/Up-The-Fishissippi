using UnityEngine;

public class TrashFlow : MonoBehaviour
{

    private Vector3 _start;
    private Vector3 _end;
    private float _flightTime;
    private float _elapsed;
    private bool _initialized;

    public void Initialize(Vector3 start, Vector3 end, float flightTime)
    {
        _start = start;
        _end = end;
        _flightTime = flightTime;
        _elapsed = 0f;
        _initialized = true;

        Vector3 direction = (_end - _start).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 10f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        if (!_initialized) return;

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / _flightTime);

        transform.position = Vector3.Lerp(_start, _end, t);

        if (t >= 1f)
            Arrive();
    }

    private void Arrive()
    {
        Destroy(gameObject);
    }
}
