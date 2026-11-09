using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceQuizUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject questionPanel;

    [Header("Text")]
    public TMP_Text questionText;
    public TMP_Text feedbackText;
    public TMP_Text studyText;

    [Header("Buttons")]
    public Button[] answerButtons;
    public Button submitButton;

    private MultipleChoiceQuestion currentQuestion;
    private int selectedChoiceIndex = -1;

    public void ShowQuestion(MultipleChoiceQuestion question)
    {
        currentQuestion = question;
        selectedChoiceIndex = -1;

        questionPanel.SetActive(true);

        questionText.text = question.questionText;
        feedbackText.text = "";
        studyText.text = "";
        submitButton.interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = i;

            answerButtons[i].gameObject.SetActive(i < question.choices.Count);

            if (i < question.choices.Count)
            {
                TMP_Text buttonText =
                    answerButtons[i].GetComponentInChildren<TMP_Text>();

                buttonText.text = question.choices[i].ToString();

                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(
                    () => SelectAnswer(buttonIndex)
                );
            }
        }
    }

    void SelectAnswer(int index)
    {
        selectedChoiceIndex = index;
        submitButton.interactable = true;

        feedbackText.text =
            "Selected answer: " + currentQuestion.choices[index];
    }

    public void SubmitAnswer()
    {
        if (selectedChoiceIndex == -1)
            return;

        bool correct =
            selectedChoiceIndex == currentQuestion.correctChoiceIndex;

        if (correct)
        {
            feedbackText.text = "Correct!";
            studyText.text = "";
        }
        else
        {
            feedbackText.text = "Not quite. " + currentQuestion.explanation;
            studyText.text = "Study: " + currentQuestion.studyTip;
        }

        submitButton.interactable = false;
    }

    public void CloseQuestion()
    {
        questionPanel.SetActive(false);
    }
}