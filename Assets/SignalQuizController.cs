using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignalQuizController : MonoBehaviour
{
    public EditableSignalGraph originalGraph;
    public EditableSignalGraph answerGraph;

    public TMP_Text questionText;
    public TMP_Text feedbackText;

    IEnumerator Start()
    {
        yield return null;

        originalGraph.SetPoints(OriginalPoints(), false);
        answerGraph.SetPoints(OriginalPoints(), true);

        questionText.text =
            "Move the key points to graph y(t) = x(2 - t).";

        feedbackText.text = "";
    }

    public void ResetAnswer()
    {
        answerGraph.SetPoints(OriginalPoints(), true);
        feedbackText.text = "";
    }

    public void SubmitAnswer()
    {
        bool correct = answerGraph.Matches(
            XOf2MinusTPoints()
        );

        feedbackText.text = correct
            ? "Correct!"
            : "Not quite. Check the reversal, shift, and jump locations.";
    }

    List<ContinuousPoint> OriginalPoints()
    {
        return new List<ContinuousPoint>
        {
            new ContinuousPoint(-5f, 0f, true),

            new ContinuousPoint(-2f, 0f, false),
            new ContinuousPoint(-2f, -1f, true),

            new ContinuousPoint(-1f, 0f, false),
            new ContinuousPoint(-1f, 1f, true),

            new ContinuousPoint(0f, 2f, false),
            new ContinuousPoint(0f, 3f, true),

            new ContinuousPoint(1f, 4f, false),
            new ContinuousPoint(1f, 3f, true),

            new ContinuousPoint(5f, 3f, false)
        };
    }

    List<ContinuousPoint> XOf2MinusTPoints()
    {
        return new List<ContinuousPoint>
        {
            new ContinuousPoint(-5f, 3f, true),

            new ContinuousPoint(1f, 3f, false),
            new ContinuousPoint(1f, 4f, true),

            new ContinuousPoint(2f, 3f, false),
            new ContinuousPoint(2f, 2f, true),

            new ContinuousPoint(3f, 1f, false),
            new ContinuousPoint(3f, 0f, true),

            new ContinuousPoint(4f, -1f, false),
            new ContinuousPoint(4f, 0f, true),

            new ContinuousPoint(5f, 0f, false)
        };
    }
}