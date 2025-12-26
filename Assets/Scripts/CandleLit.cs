using UnityEngine;
using System.Collections;

public class CandleLit : MonoBehaviour
{

    public float duration = 3f;
    [Range(0f, 1f)]
    public float startScaleFactor = 0.7f;

    Vector3 targetScale;
    public CandleLightEffect candleLightEffect; 
    

    void Awake()
    {
        targetScale = transform.localScale;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleUp());
        candleLightEffect.LightUp();
    }

    private void OnDisable()
    {
        candleLightEffect.LightOut();


    }

    IEnumerator ScaleUp()
    {
        float time = 0f;

        Vector3 startScale = targetScale * startScaleFactor;
        transform.localScale = startScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Smooth, natural growth
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
