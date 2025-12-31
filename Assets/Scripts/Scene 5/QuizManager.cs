using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI conclusionText;
    public Button[] optionButtons;
    public GameObject underlineUI;

    [Header("Menu UI")]
    public GameObject menuUI;
    public Button resetButton;   // 👈 Repeat Experiment
    public Button exitButton;     // 👈 Exit App

    [Header("Colors")]
    public Color correctColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color wrongColor = new Color(0.8f, 0.2f, 0.2f, 1f);
    public Color normalColor = Color.white;

    [Header("Quiz Data")]
    public QuizQuestion[] questions;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip wrongAnswerSFX;
    public AudioClip conclusionVO;

    private int currentQuestionIndex = 0;
    private bool answered = false;

    void Start()
    {
        conclusionText.gameObject.SetActive(false);
        menuUI.SetActive(false);

        SetupButtons();
        SetupMenuButtons();
        ShowQuestion();
    }

    void SetupButtons()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void SetupMenuButtons()
    {
        resetButton.onClick.AddListener(ResetExperiment);
        exitButton.onClick.AddListener(ExitApplication);
    }

    void ShowQuestion()
    {
        answered = false;

        QuizQuestion q = questions[currentQuestionIndex];
        questionText.text = q.question;

        questionText.gameObject.SetActive(true);
        underlineUI.SetActive(true);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].interactable = true;
            optionButtons[i].image.color = normalColor;

            optionButtons[i]
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = q.options[i];
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (answered) return;
        answered = true;

        QuizQuestion q = questions[currentQuestionIndex];

        foreach (Button b in optionButtons)
            b.interactable = false;

        optionButtons[q.correctIndex].image.color = correctColor;

        if (selectedIndex == q.correctIndex)
        {
            if (q.correctVO != null)
                audioSource.PlayOneShot(q.correctVO);
        }
        else
        {
            optionButtons[selectedIndex].image.color = wrongColor;

            if (wrongAnswerSFX != null)
                audioSource.PlayOneShot(wrongAnswerSFX);
        }

        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(4f);

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
            ShowQuestion();
        else
            EndQuiz();
    }

    void EndQuiz()
    {
        questionText.gameObject.SetActive(false);
        underlineUI.SetActive(false);

        foreach (Button b in optionButtons)
            b.gameObject.SetActive(false);

        conclusionText.gameObject.SetActive(true);
        conclusionText.text =
            "Great work!\n\n" +
            "You’ve explored Young’s Double Slit Experiment — from creating the pattern to understanding why it forms.\n\n" +
            "Interference isn’t just a lab concept. It colours soap bubbles, oil films, peacock feathers, CDs, and reduces glare on lenses.\n\n" +
            "Light is more than brightness — it’s waves creating patterns all around us.";

        if (conclusionVO != null)
            audioSource.PlayOneShot(conclusionVO);

        StartCoroutine(ActivateMenuAfterDelay(5f));
    }

    IEnumerator ActivateMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        menuUI.SetActive(true);
    }

    // =========================
    // MENU BUTTON FUNCTIONS
    // =========================

    public void ResetExperiment()
    {
        // 🔁 Full reset by reloading scene
        print("reste game");
        GameManager.Instance.LoadScene("Scene 1");
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
