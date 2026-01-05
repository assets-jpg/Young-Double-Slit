using UnityEngine;

public class TapIndicator : MonoBehaviour
{
    public float minScale = 0.003f;
    public float maxScale = 0.0036f;
    public float speed = 2f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        float scale = Mathf.Lerp(minScale, maxScale, t);

        rectTransform.localScale = new Vector3(scale, scale, 1f);
    }
}
