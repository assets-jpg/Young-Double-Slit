using UnityEngine;


public enum Scene3State
{
    WaveNature,
    DoubleSlitWaves,
    InterferenceIntro,
    PathDifference,
    Completed
}

public class Scene3_UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject wavePanel;              // Scene 3A
    public GameObject doubleSlitPanel;        // Scene 3B
    public GameObject interferencePanel;
    public GameObject constructiveInterferencePoint;
    public GameObject destructiveInterferencePoint;  // Scene 3C tablet
                                                      // Scene 3C tablet
    public GameObject signWavesPanel;
    public GameObject pathDifferencePanel;
    [Header("SCRIPTS")]
    public InterferenceController interferenceController;


    [Header("Prompts")]
    public GameObject pointFringePrompt;

    private Scene3State currentState;

    void Start()
    {
        DisableAll();

        SetState(Scene3State.WaveNature);
    }

    /* =======================
     *  STATE MACHINE
     * ======================= */

    void SetState(Scene3State newState)
    {
        currentState = newState;
        DisableAll();

        switch (currentState)
        {
            /* ---------- Scene 3A ---------- */
            case Scene3State.WaveNature:

                wavePanel.SetActive(true);
                AudioManager.Instance.PlayLightAsWave();
                Invoke(nameof(OnWaveAnimationFinished), 10f);
                break;

            /* ---------- Scene 3B ---------- */
            case Scene3State.DoubleSlitWaves:
                doubleSlitPanel.SetActive(true);
                AudioManager.Instance.PlayTwoSlitsTwoWaves();
                Invoke(nameof(OnDoubleSlitAnimationFinished), 15f);

                break;

            /* ---------- Scene 3C ---------- */
            case Scene3State.InterferenceIntro:
                doubleSlitPanel.SetActive(false);
                interferencePanel.SetActive(true);
                signWavesPanel.SetActive(true);
                AudioManager.Instance.PlayWaveInterferenceIntro();
                break;

            

            /* ---------- Scene 3D ---------- */
            case Scene3State.PathDifference:
                pathDifferencePanel.SetActive(true);
                pointFringePrompt.SetActive(true);
               // AudioManager.Instance.PlayPathDifference();
                break;

            case Scene3State.Completed:
               // AudioManager.Instance.PlayFringeSpacingIntro();
                break;
        }
    }

    void DisableAll()
    {
        wavePanel?.SetActive(false);
        doubleSlitPanel?.SetActive(false);
        interferencePanel?.SetActive(false);
        signWavesPanel.SetActive(false);

        // pathDifferencePanel?.SetActive(false);


        //pointFringePrompt?.SetActive(false);
    }

    /* =======================
     *  EVENTS FROM ANIMATION / UI
     * ======================= */

    // Called when Scene 3A animation finishes
    public void OnWaveAnimationFinished()
    {
        if (currentState == Scene3State.WaveNature)
            SetState(Scene3State.DoubleSlitWaves);
    }

    // Called when double slit wave animation finishes
    public void OnDoubleSlitAnimationFinished()
    {
        if (currentState == Scene3State.DoubleSlitWaves)
            SetState(Scene3State.InterferenceIntro);
    }


    public void SetConstructive()
    {
        interferenceController.SetConstructive();
        constructiveInterferencePoint.SetActive(true);
        destructiveInterferencePoint.SetActive(false);

    }

    public void SetDestructive()
    {
        interferenceController.SetDestructive();
        constructiveInterferencePoint.SetActive(false);
        destructiveInterferencePoint.SetActive(true);


    }
    // Called after either constructive or destructive animation
    public void OnInterferenceExampleFinished()
    {
        
            SetState(Scene3State.PathDifference);
        
    }

    // User points at first bright fringe
    public void OnFringePointed()
    {
        if (currentState == Scene3State.PathDifference)
            SetState(Scene3State.Completed);
    }
}
