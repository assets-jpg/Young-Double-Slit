using UnityEngine;
using System.Collections;

public enum Scene2State
{
    Title,
    PickupDoubleSlit,
    InsertDoubleSlit,
    TurnOnLamp,
    ObservePattern,
    Completed
}

public class Scene2_UIManager : MonoBehaviour
{
    

    [Header("Prompts (Already Designed UI)")]
    public GameObject pickupSlitPromptUI;

    public GameObject insertSlitPromptUI;
    public GameObject turnOnLampPromptUI;
    public GameObject observePatternPromptUI;

    [Header("Object Highlighters")]
    public HighLighter slitHolderHighlighter;

    public HighLighter slitPlateHighlighter;
    public HighLighter lampHighlighter;

    private Scene2State currentState;

    void Start()
    {
        DisableAllPrompts();
        SetState(Scene2State.PickupDoubleSlit);
    }

    void SetState(Scene2State newState)
    {
        currentState = newState;
        DisableAllPrompts();

        switch (currentState)
        {
            

            case Scene2State.PickupDoubleSlit:
                pickupSlitPromptUI.SetActive(true);
                slitPlateHighlighter.enabled = true;
                AudioManager.Instance.PlayPickUpDoubleSlit();

                break;

            // 🔹 STEP 1 — Insert double slit
            case Scene2State.InsertDoubleSlit:
                insertSlitPromptUI.SetActive(true);
                AudioManager.Instance.PlayInsertTheSlit();
                break;

            // 🔹 STEP 2 — Turn on lamp
            case Scene2State.TurnOnLamp:
                turnOnLampPromptUI.SetActive(true);
                lampHighlighter.enabled = true;
                AudioManager.Instance.PlayTurnOnTheLamp();
                break;

            // 🔹 STEP 3 — Observe pattern
            case Scene2State.ObservePattern:
                observePatternPromptUI.SetActive(true);
                AudioManager.Instance.PlayObserveThePattern();
                Invoke(nameof(OnUserReachedWall), 24f);

                break;

            case Scene2State.Completed:
                DisableAllPrompts();
                AudioManager.Instance.PlayLabTransition();
                Invoke(nameof(LoadNextScene), 10f);

                break;
        }
    }

    void LoadNextScene()
    {
        GameManager.Instance.LoadScene("Scene 3");

    }

  

    void DisableAllPrompts()
    {
        if (insertSlitPromptUI) insertSlitPromptUI.SetActive(false);
        if (turnOnLampPromptUI) turnOnLampPromptUI.SetActive(false);
        if (observePatternPromptUI) observePatternPromptUI.SetActive(false);
    }

 

    public void OnDoubleSlitPicked()
    {
        if (currentState == Scene2State.PickupDoubleSlit)
        {
            slitPlateHighlighter.enabled = false;
            SetState(Scene2State.InsertDoubleSlit);
            pickupSlitPromptUI.SetActive(false);
            slitHolderHighlighter.enabled = true;


        }
    }
    public void OnDoubleSlitInserted()
    {
        if (currentState == Scene2State.InsertDoubleSlit)
        {
            slitHolderHighlighter.enabled = false;

            SetState(Scene2State.TurnOnLamp);
        }
    }

    // 🔥 Lamp turned ON
    public void OnLampTurnedOn()
    {
        if (currentState == Scene2State.TurnOnLamp)
        {
            lampHighlighter.enabled = false;
            SetState(Scene2State.ObservePattern);
        }
    }

    // 👀 User walks closer to wall
    public void OnUserReachedWall()
    {
        if (currentState == Scene2State.ObservePattern)
        {
            SetState(Scene2State.Completed);
        }
    }
}
