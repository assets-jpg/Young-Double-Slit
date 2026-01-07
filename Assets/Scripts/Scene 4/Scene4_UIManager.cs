using UnityEngine;
using UnityEngine.UI;

public class Scene4_UIManager : MonoBehaviour
{
    // ---------------- UI ----------------
    [Header("Slider Panel")]
    public GameObject SliderPanel;

    [Header("Wavelength Slider")]
    public Slider wavelengthSlider;
    public Image wavelengthHandle;

    [Header("Distance Slider")]
    public Slider distanceSlider;

    [Header("Slit Separation Slider")]
    public Slider slitSeparationSlider;

    [Header("Next Button")]
    public Button nextButton;

    // ---------------- FORMULA ----------------
    [Header("Formula")]
    public GameObject FormulaPanel;

    // ---------------- VISUALS ----------------
    [Header("Light / Wave Material")]
    public Material wavelengthMaterial;

    [Header("UI Layout")]
    public VerticalLayoutGroup wavelengthLayout;

    [Header("Projection Panel")]
    public Transform projectionPanel;

    [Header("Distance Range")]
    public float minDistance = 0f;
    public float maxDistance = 2.5f;

    [Header("Layout Spacing Range")]
    public float minSpacing = 0.1f;
    public float maxSpacing = 0.3f;

    [Header("Slit GameObjects")]
    public GameObject slit1;
    public GameObject slit2;
    public GameObject slit3;

    [Header("Next Indicator")]
    public GameObject nextIndicator;


    // ---------------- DOUBLE ARROWS ----------------
    [Header("Double Arrows")]
    public DoubleArrowSingleParam wavelengthArrow;
    public DoubleArrowSingleParam distanceArrow;
    public DoubleArrowSingleParam slitArrow;

    // ---------------- MULTIPLIERS ----------------
    [Header("Arrow Multipliers")]
    public float wavelengthArrowMultiplier = 0.5f;
   

    public float distanceArrowMultiplier = 1.5f;
    public float slitArrowMultiplier = 1.0f;



    // ---------------- STATE ----------------
    enum UIStage { Wavelength, Distance, Slit }
    UIStage currentStage = UIStage.Wavelength;

    bool wavelengthTouched = false;
    bool distanceTouched = false;
    bool slitTouched = false;

    bool wavelengthExplainPlayed = false;
    bool distanceExplainPlayed = false;
    bool slitExplainPlayed = false;

    bool allSlidersExplored = false;

    // ---------------- START ----------------
    void Start()
    {
        wavelengthSlider.onValueChanged.AddListener(OnWavelengthChanged);
        distanceSlider.onValueChanged.AddListener(OnDistanceChanged);
        slitSeparationSlider.onValueChanged.AddListener(OnSlitSeparationChanged);

        nextButton.onClick.AddListener(OnNextClicked);

        wavelengthSlider.interactable = true;
        distanceSlider.interactable = false;
        slitSeparationSlider.interactable = false;
        nextButton.interactable = false;

        if (FormulaPanel)
            FormulaPanel.SetActive(false);
        if (nextIndicator)
            nextIndicator.SetActive(false);

        AudioManager.Instance.Playscene4Intro();
        Invoke(nameof(SliderPanelAppear), 10f);
    }

    void SliderPanelAppear()
    {
        SliderPanel.SetActive(true);
        AudioManager.Instance.PlaywaveLengthIntro();
    }

    // ---------------- NEXT BUTTON ----------------
    void OnNextClicked()
    {
        nextButton.interactable = false;
        
    if (nextIndicator)
        nextIndicator.SetActive(false);

        switch (currentStage)
        {
            case UIStage.Wavelength:
                currentStage = UIStage.Distance;
                distanceSlider.interactable = true;
                AudioManager.Instance.PlaydistanceIntro();
                break;

            case UIStage.Distance:
                currentStage = UIStage.Slit;
                slitSeparationSlider.interactable = true;
                AudioManager.Instance.PlayslitseperationIntro();
                break;

            case UIStage.Slit:
                if (allSlidersExplored && FormulaPanel)
                    SliderPanel.SetActive(false);

                FormulaPanel.SetActive(true);
                ApplyColor(Color.red);


                break;
        }
    }

    // ---------------- WAVELENGTH ----------------
    void OnWavelengthChanged(float value)
    {
        SnapAndApply(value);

        if (!wavelengthTouched)
        {
            wavelengthTouched = true;
            nextButton.interactable = true;

            if (nextIndicator)
                nextIndicator.SetActive(true);
        }

        switch (Mathf.RoundToInt(value))
        {
            case 1: wavelengthLayout.spacing = 0.1f; break;
            case 2: wavelengthLayout.spacing = 0.2f; break;
            case 3: wavelengthLayout.spacing = 0.3f; break;
        }

        UpdateWavelengthArrow();

        if (!wavelengthExplainPlayed)
        {
            wavelengthExplainPlayed = true;
            AudioManager.Instance.PlaywaveLengthExplain();
        }
    }

    // ---------------- DISTANCE ----------------
    void OnDistanceChanged(float value)
    {
        float xPos = Mathf.Lerp(minDistance, maxDistance, value);
        projectionPanel.localPosition = new Vector3(
            xPos,
            projectionPanel.localPosition.y,
            projectionPanel.localPosition.z
        );

        wavelengthLayout.spacing = Mathf.Lerp(minSpacing, maxSpacing, value);

        // Distance arrow
        if (distanceArrow)
        {
            distanceArrow.lengthMultiplier =
                1f + value * distanceArrowMultiplier;
        }

        // Wavelength ALSO reacts to distance (like old behavior)
        UpdateWavelengthArrow();

        if (!distanceTouched && currentStage == UIStage.Distance)
        {
            distanceTouched = true;
            nextButton.interactable = true;
            if (nextIndicator)
                nextIndicator.SetActive(true);
        }

        if (!distanceExplainPlayed)
        {
            distanceExplainPlayed = true;
            AudioManager.Instance.PlaydistanceExplain();
        }
    }

    // ---------------- SLIT ----------------
    void OnSlitSeparationChanged(float value)
    {
        int snapped = Mathf.Clamp(Mathf.RoundToInt(value), 1, 3);
        slitSeparationSlider.SetValueWithoutNotify(snapped);

        ApplySlitSeparation(snapped);

        if (slitArrow)
        {
            // Normalize AFTER snapping (1–3 → 0–1)
            float t = Mathf.InverseLerp(1f, 3f, snapped);

            // Map to 1–7
            slitArrow.lengthMultiplier = Mathf.Lerp(1f, 7f, t);
        }

        if (!slitTouched && currentStage == UIStage.Slit)
        {
            slitTouched = true;
            CheckAllSlidersExplored();
            if (nextIndicator)
                nextIndicator.SetActive(true);
        }
    }


    void ApplySlitSeparation(int step)
    {
        slit1.SetActive(step == 1);
        slit2.SetActive(step == 2);
        slit3.SetActive(step == 3);

        switch (step)
        {
            case 1: wavelengthLayout.spacing = 0.3f; break;
            case 2: wavelengthLayout.spacing = 0.2f; break;
            case 3: wavelengthLayout.spacing = 0.1f; break;
        }

        UpdateWavelengthArrow();

        if (!slitExplainPlayed)
        {
            slitExplainPlayed = true;
            AudioManager.Instance.PlayslitSeperationExplaine();
        }
    }

    // ---------------- WAVELENGTH ARROW (COMBINED LOGIC) ----------------
    void UpdateWavelengthArrow()
    {
        if (!wavelengthArrow) return;

        float spacingT =
            Mathf.InverseLerp(minSpacing, maxSpacing, wavelengthLayout.spacing);

        float distanceT = distanceSlider.value;

        // Combine wavelength + distance influence
        float combined =
    (spacingT + distanceT * 0) * 0.5f;

        wavelengthArrow.lengthMultiplier =
            1f + combined * wavelengthArrowMultiplier;
    }

    // ---------------- CHECK ----------------
    void CheckAllSlidersExplored()
    {
        if (wavelengthTouched && distanceTouched && slitTouched)
        {
            allSlidersExplored = true;
            nextButton.interactable = true;
        }
    }

    // ---------------- HELPERS ----------------
    void SnapAndApply(float value)
    {
        int snappedValue = Mathf.Clamp(Mathf.RoundToInt(value), 1, 3);
        wavelengthSlider.SetValueWithoutNotify(snappedValue);

        switch (snappedValue)
        {
            case 1: ApplyColor(Color.blue); break;
            case 2: ApplyColor(Color.green); break;
            case 3: ApplyColor(Color.red); break;
        }
    }

    void ApplyColor(Color color)
    {
        wavelengthHandle.color = color;

        if (!wavelengthMaterial) return;

        wavelengthMaterial.EnableKeyword("_EMISSION");
        wavelengthMaterial.SetColor("_EmissionColor", color * 2.5f);
        wavelengthMaterial.SetColor("_SpecColor", Color.Lerp(Color.white, color, 0.6f));
    }
    private void OnDisable()
    {
        
    }
}
