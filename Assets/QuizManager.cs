using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;

    void Start()
    {
        Debug.Log("QuizManager Start ran");

    if (quizPanel == null)
    {
        Debug.LogError("Quiz Panel has not been assigned.");
        return;
    }

    quizPanel.SetActive(true);

    Debug.Log(
        "activeSelf: " + quizPanel.activeSelf +
        " | activeInHierarchy: " + quizPanel.activeInHierarchy
    );
    }

    public void OpenQuiz()
    {
        Time.timeScale = 0f;
        quizPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseQuiz()
    {
        quizPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}