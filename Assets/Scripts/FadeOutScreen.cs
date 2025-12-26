using UnityEngine;
using System.Collections;

public class FadeOutScreen : MonoBehaviour
{
    public float fadeDuration ;

    void Start()
    {
        Invoke(nameof(StartFade), 2f);
    }

    void StartFade()
    {
        StartCoroutine(Fade());
    }


    IEnumerator Fade()
    {
        Renderer r = GetComponent<Renderer>();
        Color c = r.material.color;

        for (float a = c.a; a > 0; a -= Time.deltaTime / fadeDuration)
        {
            c.a = a;
            r.material.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
