using UnityEngine;

public class RoomCameraSettings : MonoBehaviour
{
    [Header("Master Room Zoom")]
    [Tooltip("Use 3 not 4")]
    [SerializeField] private float orthoSize = 5f;

    private int assetsPPU = 64;
    private int baseRefResX = 1138;
    private int baseRefResY = 640;

    
    [SerializeField]private int computedRefResX;
    [SerializeField]private int computedRefResY;
    private void OnValidate()
    {
        // Keep sane values
        if (assetsPPU <= 0) assetsPPU = 64;
        if (baseRefResY <= 0) baseRefResY = 640;
        if (baseRefResX <= 0) baseRefResX = 1138;
        if (orthoSize < 0.1f) orthoSize = 0.1f;

        RecomputeReferenceResolution();
    }
    private void RecomputeReferenceResolution()
    {
        // RefResY = 2 * PPU * OrthoSize
        computedRefResY = Mathf.RoundToInt(2f * assetsPPU * orthoSize);

        // Keep same aspect ratio as your baseline (1150/640)
        float aspect = (float)baseRefResX / baseRefResY;
        computedRefResX = Mathf.RoundToInt(computedRefResY * aspect);
    }

    // Your original getter (still useful for debugging)
    public float GetOrthoSize() => orthoSize;

    // New getters: what you actually feed into Pixel Perfect Camera
    public int GetReferenceResolutionX() => computedRefResX;
    public int GetReferenceResolutionY() => computedRefResY;
    public int GetAssetsPPU() => assetsPPU;
}