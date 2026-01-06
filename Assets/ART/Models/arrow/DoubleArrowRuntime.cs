using UnityEngine;

public class DoubleArrowSingleParam : MonoBehaviour
{
    [Header("Arrow Parts")]
    public Transform shaft;
    public Transform leftHead;
    public Transform rightHead; // optional

    [Header("Single Runtime Parameter")]
    [Tooltip("Controls arrow length at runtime")]
    public float lengthMultiplier = 1f;

    [Header("Tuning")]
    [Tooltip("How much head moves per unit of shaft scale")]
    public float headMoveMultiplier = 0.5f;

    float baseShaftScaleX;
    float baseLeftX;
    float baseRightX;

    bool moveBothHeads; // ← key flag

    void Start()
    {
        // Cache base values ONCE
        baseShaftScaleX = shaft.localScale.x;
        baseLeftX = leftHead.localPosition.x;

        if (rightHead)
            baseRightX = rightHead.localPosition.x;

        // Decide behavior based on GameObject name
        moveBothHeads = gameObject.name == "Slit arrow";
    }

    void Update()
    {
        UpdateArrow();
    }

    void UpdateArrow()
    {
        if (!shaft || !leftHead)
            return;

        // 1️⃣ Scale shaft on X
        Vector3 shaftScale = shaft.localScale;
        shaftScale.x = baseShaftScaleX * lengthMultiplier;
        shaft.localScale = shaftScale;

        // 2️⃣ Calculate movement delta
        float delta = (shaftScale.x - baseShaftScaleX) * headMoveMultiplier;

        // 3️⃣ Always move left head
        leftHead.localPosition = new Vector3(
            baseLeftX - delta,
            leftHead.localPosition.y,
            leftHead.localPosition.z
        );

        // 4️⃣ Move right head ONLY for slitArrow
        if (moveBothHeads && rightHead)
        {
            rightHead.localPosition = new Vector3(
                baseRightX + delta,
                rightHead.localPosition.y,
                rightHead.localPosition.z
            );
        }
    }
}
