using UnityEngine;

public class UIArrowFloat : MonoBehaviour
{
    public float amplitude = 10f;   // pixels
    public float speed = 2f;

    RectTransform rect;
    Vector2 startAnchoredPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        startAnchoredPos = rect.anchoredPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        rect.anchoredPosition = startAnchoredPos + Vector2.up * offset;
    }

    void OnDisable()
    {
        rect.anchoredPosition = startAnchoredPos;
    }
}
