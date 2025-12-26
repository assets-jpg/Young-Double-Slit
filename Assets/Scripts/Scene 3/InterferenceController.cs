using UnityEngine;

public class InterferenceController : MonoBehaviour
{
    [Header("Wave Lines")]
    public SineWaveLine wave1; // S1
    public SineWaveLine wave2; // S2

    [Header("Materials")]
    public Material blueMaterial;
    public Material redMaterial;

    [Header("Direction Offset")]
    public float constructiveDirectionOffset = 0.02f;

    [Header("Destructive Direction Y")]
    public float destructiveWave1Y = 0.2f;
    public float destructiveWave2Y = 0.25f;

    private Vector3 wave1BaseDirection;
    private Vector3 wave2BaseDirection;

    void Awake()
    {
        wave1BaseDirection = wave1.direction;
        wave2BaseDirection = wave2.direction;

       // SetConstructive();
    }

    /* =======================
     *  INTERFERENCE MODES
     * ======================= */

    // 🔵 Constructive Interference
    public void SetConstructive()
    {
        wave1.phaseOffset = 0f;
        wave2.phaseOffset = 0f;

        wave1.direction = new Vector3(
            wave1BaseDirection.x,
            -constructiveDirectionOffset,
            wave1BaseDirection.z
        );

        wave2.direction = new Vector3(
            wave2BaseDirection.x,
            +constructiveDirectionOffset,
            wave2BaseDirection.z
        );

        wave1.GetComponent<LineRenderer>().material = blueMaterial;
        wave2.GetComponent<LineRenderer>().material = blueMaterial;

        AudioManager.Instance.PlayConstructiveInterference();
    }

    // 🔴 Destructive Interference
    public void SetDestructive()
    {
        wave1.phaseOffset = 0f;
        wave2.phaseOffset = 2.75f;

        // 🔥 Specific Y directions for destructive
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

        wave1.GetComponent<LineRenderer>().material = blueMaterial;
        wave2.GetComponent<LineRenderer>().material = redMaterial;

        AudioManager.Instance.PlayDestructiveInterference();
    }
}
