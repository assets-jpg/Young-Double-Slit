using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class HighLighter : MonoBehaviour
{
    [Header("Emission Highlight")]
    public Color highlightColor = Color.cyan;
    public float blinkSpeed = 2f;
    public float intensity = 2f;
    public bool blink = true;

    Renderer rend;
    Material mat;
    Color originalEmission;
    Coroutine blinkRoutine;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material; // instance, safe

        // Cache original emission
        if (mat.HasProperty("_EmissionColor"))
            originalEmission = mat.GetColor("_EmissionColor");
        else
            originalEmission = Color.black;
    }

    void OnEnable()
    {
        EnableOutline();
    }

    void OnDisable()
    {
        DisableOutline();
    }

    // =========================
    // PUBLIC API
    // =========================

    public void EnableOutline()
    {
        if (!mat.HasProperty("_EmissionColor")) return;

        mat.EnableKeyword("_EMISSION");

        if (blink)
            blinkRoutine = StartCoroutine(BlinkEmission());
        else
            mat.SetColor("_EmissionColor", highlightColor * intensity);
    }

    public void DisableOutline()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        mat.SetColor("_EmissionColor", originalEmission);
    }

    // =========================
    // BLINK / PULSE
    // =========================

    IEnumerator BlinkEmission()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * blinkSpeed;
            float pulse = Mathf.Abs(Mathf.Sin(t));

            mat.SetColor(
                "_EmissionColor",
                highlightColor * intensity * pulse
            );

            yield return null;
        }
    }
}
