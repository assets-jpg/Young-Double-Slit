using UnityEngine;

[System.Serializable]
public class QuizQuestion
{
    [TextArea(2, 4)]
    public string question;

    public string[] options; // size = 4
    public int correctIndex;

    public AudioClip correctVO;
}
