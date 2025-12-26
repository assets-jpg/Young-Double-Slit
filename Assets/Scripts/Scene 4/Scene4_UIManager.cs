    using UnityEngine;
    using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

    public class Scene4_UIManager : MonoBehaviour
    {
        [Header("Wavelength Slider")]
        public Slider wavelengthSlider;
        public Image wavelengthHandle;   // knob image

        [Header("Light / Wave Material")]
        public Material wavelengthMaterial;
    [Header("UI Layout")]
    public VerticalLayoutGroup wavelengthLayout;

    [Header("Distance Slider")]
    public Slider distanceSlider;

    [Header("Projection Panel")]
    public Transform projectionPanel;

    [Header("Distance Range")]
    public float minDistance = 0f;
    public float maxDistance = 2.5f;

    [Header("Layout Spacing Range")]
    public float minSpacing = 0.1f;
    public float maxSpacing = 0.3f;


    void Start()
        {

            wavelengthSlider.onValueChanged.AddListener(OnWavelengthChanged);
        distanceSlider.onValueChanged.AddListener(OnDistanceChanged);
        SnapAndApply(1);

    }
    void OnDistanceChanged(float value)
    {
        // Move projection panel (Z axis)
        float xPos = Mathf.Lerp(minDistance, maxDistance, value);
        projectionPanel.localPosition = new Vector3(
            xPos,
            projectionPanel.localPosition.y,
              projectionPanel.localPosition.z
        );

        // Change UI spacing
        float spacing = Mathf.Lerp(minSpacing, maxSpacing, value);
        wavelengthLayout.spacing = spacing;

        // Force UI refresh
        //LayoutRebuilder.ForceRebuildLayoutImmediate(
        //    wavelengthLayout.GetComponent<RectTransform>()
        //);
    }

    void OnWavelengthChanged(float value)
        {
            SnapAndApply(value);
        }


        void SnapAndApply(float value)
        {
            int snappedValue = Mathf.RoundToInt(value);
            snappedValue = Mathf.Clamp(snappedValue, 1, 3);

            wavelengthSlider.SetValueWithoutNotify(snappedValue);

            switch (snappedValue)
            {
                case 1:
                    ApplyColor(Color.blue);
                    break;

                case 2:
                    ApplyColor(Color.green);
                    break;

                case 3:
                    ApplyColor(Color.red);
                    break;
            }
        }



    void ApplyColor(Color color)
    {
        // Knob color
        wavelengthHandle.color = color;

        if (wavelengthMaterial != null)
        {
            // ---------- EMISSION ----------
            wavelengthMaterial.EnableKeyword("_EMISSION");

            Color emissionColor = color * 2.5f;
            wavelengthMaterial.SetColor("_EmissionColor", emissionColor);

            // ---------- SPECULAR ----------
            Color specularColor = Color.Lerp(Color.white, color, 0.6f);
            wavelengthMaterial.SetColor("_SpecColor", specularColor);
        }

        // ---------- UI LAYOUT SPACING ----------
        if (wavelengthLayout != null)
        {
            if (color == Color.red)
                wavelengthLayout.spacing = 0.1f;
            else if (color == Color.green)
                wavelengthLayout.spacing = 0.15f;
            else if (color == Color.blue)
                wavelengthLayout.spacing = 0.2f;

            // Force layout refresh
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                wavelengthLayout.GetComponent<RectTransform>()
            );
        }
    }



}
