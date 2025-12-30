using System.Collections;
using UnityEngine;

public class FormulaAnimation : MonoBehaviour
{
    [Header("Formula Steps (4 Objects)")]
    public GameObject[] formulaObjects = new GameObject[4];

    [Header("Timing")]
    public float fadeDuration = 0.5f;
    public float intervalBetween = 0.5f;

    [Header("After Animation")]
    public float delayBeforeSceneLoad = 5f;

    private void OnEnable()
    {
        AudioManager.Instance.PlaysummeryIntro();
        Invoke(nameof(PlayAnimation), 3f);
    }

    public void PlayAnimation()
    {
        InitializeObjects();
        StartCoroutine(PlayFormulaAnimation());
    }

    // ---------------- INITIAL SETUP ----------------
    void InitializeObjects()
    {
        foreach (GameObject obj in formulaObjects)
        {
            if (!obj) continue;

            obj.SetActive(false);

            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            if (!cg)
                cg = obj.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
        }
    }

    // ---------------- MAIN ANIMATION ----------------
    IEnumerator PlayFormulaAnimation()
    {
        foreach (GameObject obj in formulaObjects)
        {
            if (!obj) continue;

            obj.SetActive(true);

            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            yield return StartCoroutine(FadeCanvasGroup(cg, 0f, 1f, fadeDuration));

            yield return new WaitForSeconds(intervalBetween);
        }

        // ✅ WAIT 5 SECONDS AFTER ANIMATION COMPLETES
        AudioManager.Instance.PlayFormulaConslusion();

        yield return new WaitForSeconds(delayBeforeSceneLoad);
        // ✅ LOAD NEXT SCENE
        GameManager.Instance.LoadScene("Scene 5");
    }

    // ---------------- FADE LOGIC ----------------
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        cg.alpha = to;
    }
}
