using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bobbingAmplitude = 0.5f;
    [SerializeField] private float bobbingFrequency = 1f;
    [SerializeField] private Transform PointLeft;
    [SerializeField] private Transform PointRight;

    private Transform targetPoint;
    private Vector2 targetpos;
    private Vector2 initialPosition;
    private bool isFlipping = false;
    private Coroutine flipCoroutine;

    private IEnumerator flip() {
        isFlipping = true;
        float elapsed = 0f;
        float duration = 0.5f; // Duration of the flip animation
        Vector3 startrotation = transform.rotation.eulerAngles;
        Vector3 endrotation = new Vector3(startrotation.x, startrotation.y - 180f, startrotation.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startrotation, endrotation, t));
            yield return null;
        }

        transform.rotation = Quaternion.Euler(endrotation);
        targetPoint = targetPoint == PointLeft ? PointRight : PointLeft;
        targetpos = targetPoint.position;
        isFlipping = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPoint = PointLeft;
        transform.position = PointRight.position;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Bobbing effect
        float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        targetpos = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        transform.position = new Vector2(targetpos.x, initialPosition.y + bobbingOffset);
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (!isFlipping)
            {
                if (flipCoroutine != null) StopCoroutine(flipCoroutine);
                flipCoroutine = StartCoroutine(flip());
            }
        }
    }
}
