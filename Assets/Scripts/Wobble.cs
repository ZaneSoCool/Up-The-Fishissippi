using UnityEngine;


public class Wobble : MonoBehaviour
{

    [SerializeField]
    public float verticalRange = 0.15f;

    [SerializeField]
    public float horizontalRange = 0.08f;

    [SerializeField]
    public float floatSpeed = 0.8f;

    private float speedVariance = 0.3f;

    private float driftChangeInterval = 2.5f;

    // ── Private state ──────────────────────────────────────────────
    private Vector3 _originPosition;
    private Vector3 _originalScale;
    private float _verticalTimer;
    private float _horizontalTimer;
    private float _verticalSpeedMult = 1f;
    private float _horizontalSpeedMult = 1f;
    private float _driftTimer = 0f;

    private float _targetVerticalMult;
    private float _targetHorizontalMult;

    void Start()
    {
        _originPosition = transform.localPosition;
        _originalScale = transform.localScale;

        _verticalTimer = Random.Range(0f, Mathf.PI * 2f);
        _horizontalTimer = Random.Range(0f, Mathf.PI * 2f);

        // Pick initial random targets
        PickNewDriftTargets();
        _verticalSpeedMult = _targetVerticalMult;
        _horizontalSpeedMult = _targetHorizontalMult;
    }

    void Update()
    {
        _driftTimer += Time.deltaTime;
        if (_driftTimer >= driftChangeInterval)
        {
            _driftTimer = 0f;
            PickNewDriftTargets();
        }

        float lerpSpeed = Time.deltaTime * (1f / driftChangeInterval);
        _verticalSpeedMult = Mathf.Lerp(_verticalSpeedMult, _targetVerticalMult, lerpSpeed);
        _horizontalSpeedMult = Mathf.Lerp(_horizontalSpeedMult, _targetHorizontalMult, lerpSpeed);

        _verticalTimer += Time.deltaTime * floatSpeed * _verticalSpeedMult;
        _horizontalTimer += Time.deltaTime * floatSpeed * _horizontalSpeedMult * 0.6f;


        float y = Mathf.Sin(_verticalTimer) * verticalRange;

        y += Mathf.Sin(_verticalTimer * 1.3f + 1.1f) * (verticalRange * 0.3f);


        float x = Mathf.Sin(_horizontalTimer + 0.7f) * horizontalRange;

        x += Mathf.Sin(_horizontalTimer * 0.7f + 2.3f) * (horizontalRange * 0.4f);

        transform.localPosition = _originPosition + new Vector3(x, y, 0f);

    }

    private void PickNewDriftTargets()
    {
        float variance = speedVariance;
        _targetVerticalMult = Random.Range(1f - variance, 1f + variance);
        _targetHorizontalMult = Random.Range(1f - variance, 1f + variance);

        if (Random.value < 0.2f) _targetHorizontalMult *= 0.2f;
        if (Random.value < 0.1f) _targetVerticalMult *= 0.3f;
    }


    public void UpdateOrigin()
    {
        _originPosition = transform.localPosition;
    }
}