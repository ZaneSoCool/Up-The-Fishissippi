using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Attackable bossAttackable;
    [SerializeField] private int segmentCount = 20;
    [SerializeField] private float segmentGap = 3f;

    private Image[] segments;
    private int maxHealth;

    void Start()
    {
        if (bossAttackable == null) return;
        maxHealth = bossAttackable.CurrentHealth;
        BuildSegments();
    }

    void BuildSegments()
    {
        HorizontalLayoutGroup layout = gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = segmentGap;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.padding = new RectOffset(4, 4, 4, 4);

        // Purple background on the panel
        GetComponent<Image>().color = Color.purple;

        segments = new Image[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject seg = new GameObject("Seg_" + i);
            seg.transform.SetParent(transform, false);
            Image img = seg.AddComponent<Image>();
            img.color = Color.purple;
            segments[i] = img;
        }
    }

    void Update()
    {
        if (bossAttackable == null || segments == null) return;

        float healthFraction = Mathf.Clamp01((float)bossAttackable.CurrentHealth / maxHealth);
        int activeSegments = Mathf.RoundToInt(healthFraction * segmentCount);

        for (int i = 0; i < segments.Length; i++)
            segments[i].color = i < activeSegments ? Color.purple : Color.white;
    }
}
