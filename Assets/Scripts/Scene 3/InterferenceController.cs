using UnityEngine;

public class InterferenceController : MonoBehaviour
{
    [Header("Wave Lines")]
    public SineWaveLine wave1; // S1
    public SineWaveLine wave2; // S2

    [Header("Materials")]
    public Material blueMaterial;
    public Material redMaterial;

    [Header("Constructive (LIVE TUNING)")]
    public float constructiveWave1Y = -0.02f;
    public float constructiveWave2Y = 0.02f;

    [Header("Destructive (LIVE TUNING)")]
    public float destructiveWave1Y = 0.2f;
    public float destructiveWave2Y = 0.25f;

    private Vector3 wave1BaseDirection;
    private Vector3 wave2BaseDirection;

    private bool isConstructive;
    private bool isDestructive;

    void Awake()
    {
        wave1BaseDirection = wave1.direction;
        wave2BaseDirection = wave2.direction;
    }

    void Update()
    {
        // Apply live tuning ONLY for the active mode
        if (isConstructive)
        {
            ApplyConstructive();
        }
        else if (isDestructive)
        {
            ApplyDestructive();
        }
    }

    /* =======================
     *  INTERFERENCE MODES
     * ======================= */

    // 🔵 Constructive Interference
    public void SetConstructive()
    {
        isConstructive = true;
        isDestructive = false;

        wave1.phaseOffset = 0f;
        wave2.phaseOffset = 0f;

        wave1.GetComponent<LineRenderer>().material = blueMaterial;
        wave2.GetComponent<LineRenderer>().material = blueMaterial;

        ApplyConstructive();
    }

    // 🔴 Destructive Interference
    public void SetDestructive()
    {
        isConstructive = false;
        isDestructive = true;

        wave1.phaseOffset = 0f;
        wave2.phaseOffset = 2.75f;

        wave1.GetComponent<LineRenderer>().material = blueMaterial;
        wave2.GetComponent<LineRenderer>().material = redMaterial;

        ApplyDestructive();
    }

    /* =======================
     *  APPLY METHODS
     * ======================= */

    void ApplyConstructive()
    {
        wave1.direction = new Vector3(
            wave1BaseDirection.x,
            constructiveWave1Y,
            wave1BaseDirection.z
        );

        wave2.direction = new Vector3(
            wave2BaseDirection.x,
            constructiveWave2Y,
            wave2BaseDirection.z
        );
    }

    void ApplyDestructive()
    {
        wave1.direction = new Vector3(
            wave1BaseDirection.x,
            destructiveWave1Y,
            wave1BaseDirection.z
        );

        wave2.direction = new Vector3(
            wave2BaseDirection.x,
            destructiveWave2Y,
            wave2BaseDirection.z
        );
    }
}
