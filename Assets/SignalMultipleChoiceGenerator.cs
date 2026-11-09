using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignalMultipleChoiceGenerator : MonoBehaviour
{
    public MultipleChoiceQuizUI quizUI;

    
    private Dictionary<int, int> originalSignal =
        new Dictionary<int, int>()
        {
            { -3, 0 }, 
            { -2, 0 },
            { -1, 0 },
            { 0, 0 },
            { 1, 1 },
            { 2, 2 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 }
        };

    public void OpenRandomTransformationQuestion()
    {
        int[] possibleQuestionIndices = {-3, -2, -1, 0, 1, 2, 3 };
        int askedN = possibleQuestionIndices[
            Random.Range(0, possibleQuestionIndices.Length)
        ];

        bool useReverseAndShift = Random.value > 0.5f;

        int sourceIndex;
        string transformationText;

        if (useReverseAndShift)
        {
            
            sourceIndex = -askedN + 4;
            transformationText = "y(t) = x(-t + 4)";
        }
        else
        {
            
            sourceIndex = askedN - 2;
            transformationText = "y(t) = x(t - 2)";
        }

        int correctValue = GetSignalValue(sourceIndex);

        MultipleChoiceQuestion question =
            new MultipleChoiceQuestion();

        question.questionText =
            "Given the original signal:\n" +
            "x(t) = u(t) + u(t-1) - 2*u(t-2)\n\n" +
            "What is y(" + askedN + ") if " +
            transformationText + "?";

        question.choices = CreateChoices(correctValue);

        question.correctChoiceIndex =
            question.choices.IndexOf(correctValue);

        question.explanation =
            "First, transform the original signal as needed. " + "Recall that subtracting means shifting to the right " +
            "and adding means shifting to the left. " +
            "Substitute n = " + askedN +
            " into the transformation. This gives x(" +
            sourceIndex + ") = " + correctValue + ".";

        question.studyTip =
            "Review time reversal and time shifting of discrete-time signals.";

        quizUI.ShowQuestion(question);
    }

    int GetSignalValue(int n)
    {
        if (originalSignal.ContainsKey(n))
            return originalSignal[n];

        return 0;
    }

    List<int> CreateChoices(int correctValue)
    {
        List<int> choices = new List<int>
        {
            correctValue,
            correctValue + 1,
            correctValue - 1,
            correctValue + 2
        };

        // Shuffle choices so the correct answer is not always first.
        for (int i = 0; i < choices.Count; i++)
        {
            int randomIndex = Random.Range(i, choices.Count);

            int temporary = choices[i];
            choices[i] = choices[randomIndex];
            choices[randomIndex] = temporary;
        }

        return choices;
    }

    //temporary, remove once added in-game ability to make question show up, but for now: press q
    void Update()
{
    if (Keyboard.current != null &&
        Keyboard.current.qKey.wasPressedThisFrame)
    {
        OpenRandomTransformationQuestion();
    }
}
}