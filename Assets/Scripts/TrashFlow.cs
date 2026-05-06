using UnityEngine;

public class TrashFlow : MonoBehaviour
{

    private Vector3 _start;
    private Vector3 _end;
    private float _flightTime;
    private float _elapsed;
    private bool _initialized;

    private float bounceStrength = 2f;

    private int trashFlowDamage;

    Rigidbody2D playerRB;

    public void Initialize(Vector3 start, Vector3 end, float flightTime)
    {
        _start = start;
        Vector3 direction = (end - start).normalized;
        float spriteHalfLength = 5f;
        _end = new Vector3(end.x, start.y, end.z) + direction * spriteHalfLength;
        _flightTime = flightTime;
        _elapsed = 0f;
        _initialized = true;

        bool travellingRight = end.x > start.x;
        transform.rotation = Quaternion.Euler(0f, 0f, travellingRight ? 0f : 180f);
    }

    private void Update()
    {
        if (!_initialized) return;

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / _flightTime);

        transform.position = Vector3.Lerp(_start, _end, t);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);

        if (t >= 1f)
            Arrive();
    }

    private void Arrive()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRB.linearVelocity *= -bounceStrength;
            Attackable playerAttackableScript = playerRB.gameObject.GetComponent<Attackable>();
            playerAttackableScript.Attacked(trashFlowDamage);
        }

    }
}
