using UnityEngine;

public class DoubleArrowSingleParam : MonoBehaviour
{
    [Header("Arrow Parts")]
    public Transform shaft;
    public Transform leftHead;
    public Transform rightHead;

    [Header("Single Runtime Parameter")]
    [Tooltip("Controls arrow length at runtime")]
    public float lengthMultiplier = 1f;

    [Header("Tuning")]
    [Tooltip("How much head moves per unit of shaft scale")]
    public float headMoveMultiplier = 0.5f;

    float baseShaftScaleX;
    float baseLeftX;
    float baseRightX;

    void Start()
    {
        // Cache base values ONCE
        baseShaftScaleX = shaft.localScale.x;
        baseLeftX = leftHead.localPosition.x;
        baseRightX = rightHead.localPosition.x;
    }

    void Update()
    {
        UpdateArrow();
    }

    void UpdateArrow()
    {
        if (!shaft || !leftHead || !rightHead)
            return;

        // 1️⃣ Scale shaft on X
        Vector3 shaftScale = shaft.localScale;
        shaftScale.x = baseShaftScaleX * lengthMultiplier;
        shaft.localScale = shaftScale;

        // 2️⃣ Move heads based on multiplier delta
        float delta = (shaftScale.x - baseShaftScaleX) * headMoveMultiplier;

        leftHead.localPosition = new Vector3(
            baseLeftX - delta,
            leftHead.localPosition.y,
            leftHead.localPosition.z
        );

        rightHead.localPosition = new Vector3(
            baseRightX + delta,
            rightHead.localPosition.y,
            rightHead.localPosition.z
        );
    }
}
