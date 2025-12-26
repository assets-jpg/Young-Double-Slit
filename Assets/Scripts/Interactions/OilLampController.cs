using UnityEngine;
using Unity.VRTemplate;
using System.Collections;

public class OilLampController : MonoBehaviour
{
    [Header("XR")]
    public XRKnob xrKnob;
    [Header("Ui manager script")]
    public Scene2_UIManager scene2_UIManager;

    [Header("Wick")]
    public Transform wick;
    public float wickStartZ = 0f;
    public float wickEndZ = 0.03f;

    [Header("Flame")]
    public GameObject flameObject;
    public float flameIgnitePoint = 0.3f;

    [Header("Lighting")]
    public CandleLightEffect candleLight;

    [Header("Light Beam")]
    public GameObject lightCone;
    public float beamMaxScale = 1.5f;
    public float beamGrowTime = 1.2f;

    [Header("Wall Pattern")]
    public GameObject wallPattern;

    private Vector3 beamBaseScale;
    private Coroutine beamRoutine;
    private bool lampIsOn;

    private void Awake()
    {
        flameObject.SetActive(false);
        lampIsOn = false;

        if (lightCone != null)
        {
            beamBaseScale = lightCone.transform.localScale;
            lightCone.transform.localScale = Vector3.zero;
            lightCone.SetActive(false);
        }

        if (wallPattern != null)
            wallPattern.SetActive(false);
    }

    private void Update()
    {
        float knobValue = xrKnob.value;
        MoveWick(knobValue);
        HandleFlameAndLight(knobValue);
    }

    private void MoveWick(float t)
    {
        Vector3 pos = wick.localPosition;
        pos.z = Mathf.Lerp(wickStartZ, wickEndZ, t);
        wick.localPosition = pos;
    }

    private void HandleFlameAndLight(float t)
    {
        bool shouldBeOn = t >= flameIgnitePoint;

        if (shouldBeOn && !lampIsOn)
        {
            lampIsOn = true;
            flameObject.SetActive(true);

            candleLight?.LightUp();
            StartBeam();
        }
        else if (!shouldBeOn && lampIsOn)
        {
            lampIsOn = false;
            flameObject.SetActive(false);

            candleLight?.LightOut();
            StopBeam();
        }
    }

    /* =======================
     *  BEAM CONTROL
     * ======================= */

    private void StartBeam()
    {
        if (lightCone == null) return;

        if (beamRoutine != null)
            StopCoroutine(beamRoutine);

        lightCone.SetActive(true);
        beamRoutine = StartCoroutine(BeamGrow());
    }

    private void StopBeam()
    {
        if (lightCone == null) return;

        if (beamRoutine != null)
            StopCoroutine(beamRoutine);

        lightCone.transform.localScale = Vector3.zero;
        lightCone.SetActive(false);

        if (wallPattern != null)
            wallPattern.SetActive(false);
    }

    /* =======================
     *  BEAM ANIMATION
     * ======================= */

    private IEnumerator BeamGrow()
    {
        float t = 0f;
        Vector3 targetScale = Vector3.one * beamMaxScale;

        while (t < beamGrowTime)
        {
            t += Time.deltaTime;
            float normalized = t / beamGrowTime;

            float eased = Mathf.SmoothStep(0f, 1f, normalized);

            lightCone.transform.localScale =
                Vector3.Lerp(Vector3.zero, targetScale, eased);

            yield return null;
        }

        // Ensure final scale
        lightCone.transform.localScale = targetScale;

        // 🔥 Enable wall pattern when beam completes
        if (wallPattern != null)
            wallPattern.SetActive(true);
        scene2_UIManager.OnLampTurnedOn();
    }
}
