using UnityEngine;
using System.Collections;

public enum Scene3State
{
    WaveNature,
    DoubleSlitWaves,
    InterferenceIntro,
    Completed
}

public class Scene3_UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject wavePanel;
    public GameObject doubleSlitPanel;
    public GameObject interferenceTypeUI;
    public GameObject interferenceFringePanel;
    public GameObject signWavesPanel;

    [Header("Scripts")]
    public InterferenceController interferenceController;

    [Header("Holograms")]
    public GameObject hologram1;
    public GameObject hologram2;

    [Header("Interference Settings")]
    public float nextHologramDelay = 10f;

    private Scene3State currentState;

    // 🔹 NEW FLAGS
    private bool constructiveExplored = false;
    private bool destructiveExplored = false;
    private bool nextHologramCalled = false;

    private void Start()
    {
        AudioManager.Instance.PlayBeginExperiment();

    }
    public void Begin()
    {
        DisableAll();
        SetState(Scene3State.WaveNature);
    }

    /* =======================
     *  STATE MACHINE
     * ======================= */

    void SetState(Scene3State newState)
    {
        StopAllCoroutines();

        currentState = newState;
        DisableAll();

        // Reset exploration flags when entering interference
        if (newState == Scene3State.InterferenceIntro)
        {
            constructiveExplored = false;
            destructiveExplored = false;
            nextHologramCalled = false;
        }

        switch (currentState)
        {
            case Scene3State.WaveNature:
                wavePanel.SetActive(true);
                AudioManager.Instance.PlayLightAsWave();
                Invoke(nameof(OnWaveAnimationFinished), 10f);
                break;

            case Scene3State.DoubleSlitWaves:
                doubleSlitPanel.SetActive(true);
                interferenceFringePanel.SetActive(true);
                AudioManager.Instance.PlayTwoSlitsTwoWaves();
                ActivateHologramAfterDelay(hologram1, 15f);

                break;

            case Scene3State.InterferenceIntro:
                interferenceTypeUI.SetActive(true);
                signWavesPanel.SetActive(true);
                interferenceFringePanel.SetActive(true);

                AudioManager.Instance.PlayWaveInterferenceIntro();
                AudioManager.Instance.PlayExploreInterefence();
                break;

            case Scene3State.Completed:
                GameManager.Instance.LoadScene("Scene 4");
                break;
        }
    }

    /* =======================
     *  HOLOGRAM DELAY
     * ======================= */

    void ActivateHologramAfterDelay(GameObject hologram, float delay)
    {
        if (hologram == null) return;
        StartCoroutine(ActivateAfterDelay(hologram, delay));
    }

    IEnumerator ActivateAfterDelay(GameObject hologram, float delay)
    {
        yield return new WaitForSeconds(delay);
       
        hologram.SetActive(true);
    }

    /* =======================
     *  INTERFERENCE LOGIC
     * ======================= */

    public void SetConstructive()
    {
        interferenceController.SetConstructive();

        constructiveExplored = true;
        CheckInterferenceCompletion();
        AudioManager.Instance.PlayConstructiveInterference();

    }

    public void SetDestructive()
    {
        interferenceController.SetDestructive();

        destructiveExplored = true;
        CheckInterferenceCompletion();
        AudioManager.Instance.PlayDestructiveInterference();

    }

    void CheckInterferenceCompletion()
    {
        if (constructiveExplored && destructiveExplored && !nextHologramCalled)
        {
            nextHologramCalled = true;
            StartCoroutine(CallNextHologramAfterDelay());
        }
    }

    IEnumerator CallNextHologramAfterDelay()
    {
        yield return new WaitForSeconds(nextHologramDelay);
        hologram2.SetActive(true);
    }

    /* =======================
     *  HELPERS
     * ======================= */

    void DisableAll()
    {
       // wavePanel?.SetActive(false);
        doubleSlitPanel?.SetActive(false);
        interferenceFringePanel?.SetActive(false);
        signWavesPanel?.SetActive(false);
        hologram1?.SetActive(false);
        hologram2?.SetActive(false);
    }

    /* =======================
     *  STATE CALLBACKS
     * ======================= */

    public void OnWaveAnimationFinished()
    {
        if (currentState == Scene3State.WaveNature)
            SetState(Scene3State.DoubleSlitWaves);
    }

    public void OnInterferenceIntroFinished()
    {
        if (currentState == Scene3State.DoubleSlitWaves)
            SetState(Scene3State.InterferenceIntro);
    }

    public void HologramTwoTouch()
    {
        AudioManager.Instance.Playscene3to4VO();

        Invoke(nameof(OnComplete), 6f);
    }

    public void OnComplete()
    {
        SetState(Scene3State.Completed);
    }
}
