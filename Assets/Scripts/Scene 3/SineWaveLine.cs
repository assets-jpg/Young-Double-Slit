using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SineWaveLine : MonoBehaviour
{
    [Header("Line Shape")]
    public int points = 120;
    public float lineLength = 2.55f;   // 🔥 controls how long the wave is

    [Header("Wave Shape")]
    public float amplitude = 0.15f;
    public float wavelength = 1.0f;

    [Header("Wave Motion")]
    public float speed = 1.5f;
    public float phaseOffset = 0f;

    [Header("Direction")]
    public Vector3 direction = Vector3.right;

    private LineRenderer line;
    private float time;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = points;
    }

    void Update()
    {
        time += Time.deltaTime * speed;

        float step = lineLength / (points - 1);

        for (int i = 0; i < points; i++)
        {
            float x = step * i;

            float y = Mathf.Sin(
                (x / wavelength) * Mathf.PI * 2f +
                time +
                phaseOffset
            ) * amplitude;

            Vector3 pos =
                transform.position +
                direction.normalized * x +
                transform.up * y;

            line.SetPosition(i, pos);
        }
    }
}
