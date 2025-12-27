using UnityEngine;
using UnityEngine.UI;

public class Scene4_UIManager : MonoBehaviour
{
    // ---------------- UI ----------------
    [Header("Slider panel ")]
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

    // ---------------- STATE ----------------
    enum UIStage
    {
        Wavelength,
        Distance,
        Slit
    }

    UIStage currentStage = UIStage.Wavelength;

    bool wavelengthTouched = false;
    bool distanceTouched = false;

    // ---------------- START ----------------
    void Start()
    {
        // Slider listeners
        wavelengthSlider.onValueChanged.AddListener(OnWavelengthChanged);
        distanceSlider.onValueChanged.AddListener(OnDistanceChanged);
        slitSeparationSlider.onValueChanged.AddListener(OnSlitSeparationChanged);

        // Next button
        nextButton.onClick.AddListener(OnNextClicked);

        // Initial UI state
        wavelengthSlider.interactable = true;
        distanceSlider.interactable = false;
        slitSeparationSlider.interactable = false;
        nextButton.interactable = false;
        AudioManager.Instance.Playscene4Intro();
        Invoke(nameof(SliderPanelAppear), 10f);

    }

    public void SliderPanelAppear()
    {
        SliderPanel.SetActive(true);
        AudioManager.Instance.PlaywaveLengthIntro();

    }

    // ---------------- NEXT BUTTON ----------------
    void OnNextClicked()
    {
        nextButton.interactable = false;

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
        }

        switch (Mathf.RoundToInt(value))
        {
            case 1: wavelengthLayout.spacing = 0.1f; break;
            case 2: wavelengthLayout.spacing = 0.2f; break;
            case 3: wavelengthLayout.spacing = 0.3f; break;
        }
        AudioManager.Instance.PlaywaveLengthExplain();

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

        float spacing = Mathf.Lerp(minSpacing, maxSpacing, value);
        wavelengthLayout.spacing = spacing;

        if (!distanceTouched && currentStage == UIStage.Distance)
        {
            distanceTouched = true;
            nextButton.interactable = true;
        }
        AudioManager.Instance.PlaydistanceExplain();

    }

    // ---------------- SLIT ----------------
    void OnSlitSeparationChanged(float value)
    {
        int snapped = Mathf.Clamp(Mathf.RoundToInt(value), 1, 3);
        slitSeparationSlider.SetValueWithoutNotify(snapped);
        ApplySlitSeparation(snapped);

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
        AudioManager.Instance.PlayslitSeperationExplaine();

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

        Color specular = Color.Lerp(Color.white, color, 0.6f);
        wavelengthMaterial.SetColor("_SpecColor", specular);
    }
}
