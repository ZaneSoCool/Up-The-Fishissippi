using UnityEngine;

public class DistanceTraveledTracker : MonoBehaviour
{
    [SerializeField]
    private bool trackOnStart = true;

    [SerializeField]
    private float totalDistance = 0f;

    private Vector3 lastPostion;
    private bool isTracking = false;

    public float TotalDistance => totalDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPostion = transform.position;
        if (trackOnStart) isTracking = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTracking) return;

        float distanceThisFrame = Vector3.Distance(transform.position, lastPostion);
        totalDistance += distanceThisFrame;
        lastPostion = transform.position;

    }

    public void ResetDistance()
    {
        totalDistance = 0f;
        lastPostion = transform.position;
    }
}
