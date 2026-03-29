using UnityEngine;

public class RoomCameraSettings : MonoBehaviour
{
    [SerializeField]
    private float OrthoSize = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetOrthoSize()
    {
        return OrthoSize;
    }
}
