using UnityEngine;

public class WaveSphere : MonoBehaviour
{
    [Header("Wave Settings")]
    public float expandSpeed = 2f;
    public float lifeTime = 4f;

    [Header("Fade")]
    public float fadeInTime = 0.4f;
    public float fadeOutTime = 0.6f;
    public float maxAlpha = 0.2f;

    private Material mat;
    private float age;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        // Start invisible
        Color c = mat.color;
        c.a = 0f;
        mat.color = c;
    }

    void Update()
    {
        age += Time.deltaTime;

        // Expand uniformly in 3D
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        float alpha;

        // Fade IN
        if (age < fadeInTime)
        {
            alpha = Mathf.Lerp(0f, maxAlpha, age / fadeInTime);
        }
        // Fade OUT
        else if (age > lifeTime - fadeOutTime)
        {
            float t = (age - (lifeTime - fadeOutTime)) / fadeOutTime;
            alpha = Mathf.Lerp(maxAlpha, 0f, t);
        }
        // Fully visible
        else
        {
            alpha = maxAlpha;
        }

        Color c = mat.color;
        c.a = alpha;
        mat.color = c;

        if (age >= lifeTime)
            Destroy(gameObject);
    }
}
