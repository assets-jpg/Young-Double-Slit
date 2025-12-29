using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Scene1State
{
    Title,
    PickLighter,
    LightCandle,
    RotateObject,
    Completed
}


public class Scene1_UIManager : MonoBehaviour
{
    [Header("Title")]
    public GameObject titleObject;
    public float titleDuration = 8f;

    [Header("Prompts (Already Designed UI)")]
    public GameObject lighterPromptUI;
    public GameObject candlePromptUI;
    public GameObject objectPromptUI;

    

    [Header("Objects HighLighter")]

    public HighLighter lighterHighLighter;
    public HighLighter cubeHighLighter;


    private Scene1State currentState;



    void Start()
    {
        DisableAllPrompts();
        SetState(Scene1State.Title);
        
    }

    void SetState(Scene1State newState)
    {
        currentState = newState;
        DisableAllPrompts();

        switch (currentState)
        {
            case Scene1State.Title:
                StartCoroutine(TitleRoutine());
                break;

            case Scene1State.PickLighter:
                lighterPromptUI.SetActive(true);
                lighterHighLighter.enabled = true;
                break;

            case Scene1State.LightCandle:
                candlePromptUI.SetActive(true);
                break;

            case Scene1State.RotateObject:
                objectPromptUI.SetActive(true);
               cubeHighLighter.enabled = true;
                AudioManager.Instance.PlayRotateTheObj();


                break;

            case Scene1State.Completed:
                objectPromptUI.SetActive(false);
                AudioManager.Instance.PlayNatureOfLight();
                Invoke(nameof(LoadNextScene), 20f);

                break;
        }
    }

    IEnumerator TitleRoutine()
    {
        yield return new WaitForSeconds(4f);

        titleObject.SetActive(true);
        AudioManager.Instance.PlayIntro();

        yield return new WaitForSeconds(titleDuration);

        titleObject.SetActive(false);
        SetState(Scene1State.PickLighter);
    }

    void DisableAllPrompts()
    {
        if (lighterPromptUI) lighterPromptUI.SetActive(false);
        if (candlePromptUI) candlePromptUI.SetActive(false);
        if (objectPromptUI) objectPromptUI.SetActive(false);
    }

   

    // ===== EVENTS FROM INTERACTIONS =====
    public void OnLighterPicked()
    {
        if (currentState == Scene1State.PickLighter)
            SetState(Scene1State.LightCandle);
        lighterHighLighter.enabled = false;

    }
    public void OnCubePicked()
    {
        if (currentState == Scene1State.RotateObject)
            SetState(Scene1State.Completed);
        cubeHighLighter.enabled = false;
       

    }

    public void OnCandleLit()
    {
        if (currentState == Scene1State.LightCandle)
            SetState(Scene1State.RotateObject);
    }

    void LoadNextScene()
    {
        GameManager.Instance.LoadScene("Scene 2");

    }


}
