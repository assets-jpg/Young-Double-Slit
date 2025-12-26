using UnityEngine;
using System.Collections;

public class CandleLightEffect : MonoBehaviour
{
    /* =======================
     *  INSPECTOR SETTINGS
     * ======================= */

    [Header("Candle Light (Point Light)")]
    public Light candleLight;   // PROVIDED BY YOU

    [Header("Ignition")]
    public float igniteDuration = 1.2f;
    public float targetRange = 3f;

    /* =======================
     *  HARD-CODED TUNED VALUES
     * ======================= */

    private const float DIRECTIONAL_FADE_TIME = 1.2f;

    private const float FLICKER_SPEED = 1.8f;
    private const float INTENSITY_VARIATION = 0.2f;
    private const float RANGE_VARIATION = 0.15f;

    private const float COLOR_DRIFT_AMOUNT = 0.06f;
    private const float COLOR_DRIFT_SPEED = 0.8f;

    /* =======================
     *  INTERNAL STATE
     * ======================= */

    private Light directionalLight;

    private float baseIntensity;
    private float directionalStartIntensity;
    private Color baseColor;

    private float igniteTimer;
    private bool isIgnited;
    private bool isActive;

    private float noiseSeed;

    private Coroutine directionalFadeRoutine;

    /* =======================
     *  UNITY METHODS
     * ======================= */

    private void Awake()
    {
        if (candleLight == null)
        {
            Debug.LogError("CandleLightEffect: Candle Light is not assigned.");
            enabled = false;
            return;
        }

        // Cache candle base values
        baseIntensity = candleLight.intensity;
        baseColor = candleLight.color;

        // Random flicker offset
        noiseSeed = Random.value * 10f;

        // Get directional light from Sun
        directionalLight = RenderSettings.sun;
        if (directionalLight != null)
            directionalStartIntensity = directionalLight.intensity;

        candleLight.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (!isIgnited)
        {
            IgniteCandle();
            return;
        }

        FlickerLight();
        DriftColor();
    }

    /* =======================
     *  PUBLIC API
     * ======================= */

    /// <summary>
    /// Turns the candle on and dims the sun
    /// </summary>
    public void LightUp()
    {
        candleLight.gameObject.SetActive(true);

        isActive = true;
        isIgnited = false;
        igniteTimer = 0f;

        candleLight.color = baseColor;

        if (directionalLight != null)
            StartDirectionalFade(0f);
    }

    /// <summary>
    /// Turns the candle off and restores the sun
    /// </summary>
    public void LightOut()
    {
        candleLight.gameObject.SetActive(false);

        isActive = false;

        candleLight.intensity = 0f;
        candleLight.range = 0f;

        if (directionalLight != null)
            StartDirectionalFade(directionalStartIntensity);
    }

    /* =======================
     *  CANDLE BEHAVIOR
     * ======================= */

    private void IgniteCandle()
    {
        igniteTimer += Time.deltaTime;
        float t = Mathf.Clamp01(igniteTimer / igniteDuration);

        candleLight.intensity = Mathf.Lerp(0f, baseIntensity, t);
        candleLight.range = Mathf.Lerp(0f, targetRange, t);

        if (t >= 1f)
            isIgnited = true;
    }

    private void FlickerLight()
    {
        float noise =
            Mathf.PerlinNoise(Time.time * FLICKER_SPEED, noiseSeed) - 0.5f;

        candleLight.intensity =
            baseIntensity + noise * INTENSITY_VARIATION;

        candleLight.range =
            targetRange + noise * RANGE_VARIATION;
    }

    private void DriftColor()
    {
        float colorNoise =
            Mathf.Sin(Time.time * COLOR_DRIFT_SPEED) * COLOR_DRIFT_AMOUNT;

        candleLight.color = new Color(
            Mathf.Clamp01(baseColor.r + colorNoise),
            Mathf.Clamp01(baseColor.g - Mathf.Abs(colorNoise) * 0.4f),
            Mathf.Clamp01(baseColor.b - Mathf.Abs(colorNoise))
        );
    }

    /* =======================
     *  DIRECTIONAL LIGHT CONTROL
     * ======================= */

    private void StartDirectionalFade(float targetIntensity)
    {
        if (directionalFadeRoutine != null)
            StopCoroutine(directionalFadeRoutine);

        directionalFadeRoutine =
            StartCoroutine(FadeDirectionalLight(targetIntensity));
    }

    private IEnumerator FadeDirectionalLight(float targetIntensity)
    {
        float startIntensity = directionalLight.intensity;
        float time = 0f;

        while (time < DIRECTIONAL_FADE_TIME)
        {
            time += Time.deltaTime;
            float t = time / DIRECTIONAL_FADE_TIME;

            directionalLight.intensity =
                Mathf.Lerp(startIntensity, targetIntensity, t);

            yield return null;
        }

        directionalLight.intensity = targetIntensity;
    }
}
