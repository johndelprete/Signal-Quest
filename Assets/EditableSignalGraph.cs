using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ContinuousPoint
{
    public float t;
    public float value;

    // False means there is a jump after this point.
    public bool connectToNext;

    public ContinuousPoint(float time, float yValue, bool connect)
    {
        t = time;
        value = yValue;
        connectToNext = connect;
    }
}

public class EditableSignalGraph : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform plotArea;
    public RectTransform linesParent;
    public RectTransform handlesParent;
    public Image linePrefab;
    public ContinuousGraphHandle handlePrefab;
    public TMP_Text coordinateText;

    [Header("Graph Range")]
    public float minT = -5f;
    public float maxT = 5f;
    public float minValue = -5f;
    public float maxValue = 5f;

    [Header("Display")]
    public bool showHandles = true;
    public float lineThickness = 4f;

    private List<ContinuousPoint> points = new List<ContinuousPoint>();
    private List<ContinuousGraphHandle> handleViews =
        new List<ContinuousGraphHandle>();

    private List<SegmentView> segmentViews =
        new List<SegmentView>();

    private class SegmentView
    {
        public int startPointIndex;
        public Image image;
    }

    public void SetPoints(List<ContinuousPoint> newPoints, bool editable)
    {
        Debug.Log("SetPoints received " + newPoints.Count + " points on " + gameObject.name);
        points = CopyPoints(newPoints);

        ClearViews();
        BuildViews(editable);
        UpdateViews();
    }

    List<ContinuousPoint> CopyPoints(List<ContinuousPoint> source)
    {
        List<ContinuousPoint> copy = new List<ContinuousPoint>();

        foreach (ContinuousPoint point in source)
        {
            copy.Add(new ContinuousPoint(
                point.t,
                point.value,
                point.connectToNext
            ));
        }

        return copy;
    }

    void ClearViews()
    {
        foreach (Transform child in linesParent)
            Destroy(child.gameObject);

        foreach (Transform child in handlesParent)
            Destroy(child.gameObject);

        handleViews.Clear();
        segmentViews.Clear();
    }

    void BuildViews(bool editable)
    {
        Debug.Log("BuildViews ran on " + gameObject.name);
        for (int i = 0; i < points.Count; i++)
        {
            if (showHandles)
            {
                ContinuousGraphHandle handle =
                    Instantiate(handlePrefab, handlesParent);

                PrepareRect(handle.GetComponent<RectTransform>());
                handle.Setup(this, i, editable);
                handleViews.Add(handle);
            }

            Debug.Log("Point " + i + ": connectToNext = " + points[i].connectToNext);
            if (i < points.Count - 1 && points[i].connectToNext)
{
    Image line = Instantiate(linePrefab, linesParent);

    Debug.Log(
        "Created a line under: " + line.transform.parent.name +
        " | child count: " + linesParent.childCount
    );

    PrepareRect(line.rectTransform);

    segmentViews.Add(new SegmentView
    {
        startPointIndex = i,
        image = line
    });
}
        }
    }

    void PrepareRect(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = Vector3.one;
    }

    public void MovePointFromPointer(int pointIndex, PointerEventData eventData)
{
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        plotArea,
        eventData.position,
        eventData.pressEventCamera,
        out Vector2 localPosition
    );

    float normalizedX = Mathf.InverseLerp(
        -plotArea.rect.width / 2f,
         plotArea.rect.width / 2f,
        localPosition.x
    );

    float normalizedY = Mathf.InverseLerp(
        -plotArea.rect.height / 2f,
         plotArea.rect.height / 2f,
        localPosition.y
    );

    float newT = Mathf.Lerp(minT, maxT, normalizedX);
    float newValue = Mathf.Lerp(minValue, maxValue, normalizedY);

    // Keeps points on whole-number time/value coordinates.
    newT = Mathf.Clamp(Mathf.Round(newT), minT, maxT);
    newValue = Mathf.Clamp(Mathf.Round(newValue), minValue, maxValue);

    points[pointIndex].t = newT;
    points[pointIndex].value = newValue;

    UpdateViews();
    //ShowCoordinate(newT, newValue);
}
    void UpdateViews()
    {
        for (int i = 0; i < handleViews.Count; i++)
        {
            if (i < points.Count)
            {
                handleViews[i].SetPosition(
                    TimeValueToLocal(points[i].t, points[i].value)
                );
            }
        }

        foreach (SegmentView segment in segmentViews)
        {
            Vector2 start = TimeValueToLocal(
                points[segment.startPointIndex].t,
                points[segment.startPointIndex].value
            );

            Vector2 end = TimeValueToLocal(
                points[segment.startPointIndex + 1].t,
                points[segment.startPointIndex + 1].value
            );

            Vector2 difference = end - start;
            RectTransform rect = segment.image.rectTransform;

            rect.anchoredPosition = (start + end) / 2f;
            rect.sizeDelta = new Vector2(
                difference.magnitude,
                lineThickness
            );

            float angle = Mathf.Atan2(
                difference.y,
                difference.x
            ) * Mathf.Rad2Deg;

            rect.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    Vector2 TimeValueToLocal(float t, float value)
    {
        float x = Mathf.Lerp(
            -plotArea.rect.width / 2f,
            plotArea.rect.width / 2f,
            Mathf.InverseLerp(minT, maxT, t)
        );

        float y = Mathf.Lerp(
            -plotArea.rect.height / 2f,
            plotArea.rect.height / 2f,
            Mathf.InverseLerp(minValue, maxValue, value)
        );

        return new Vector2(x, y);
    }

    float LocalXToTime(float localX)
    {
        float normalized = Mathf.InverseLerp(
            -plotArea.rect.width / 2f,
            plotArea.rect.width / 2f,
            localX
        );

        return Mathf.Clamp(
            Mathf.Lerp(minT, maxT, normalized),
            minT,
            maxT
        );
    }

    float LocalYToValue(float localY)
    {
        float normalized = Mathf.InverseLerp(
            -plotArea.rect.height / 2f,
            plotArea.rect.height / 2f,
            localY
        );

        return Mathf.Clamp(
            Mathf.Lerp(minValue, maxValue, normalized),
            minValue,
            maxValue
        );
    }

    void ShowCoordinate(ContinuousPoint point)
    {
        if (coordinateText != null)
        {
            coordinateText.gameObject.SetActive(true);
            coordinateText.text =
                "t = " + point.t.ToString("F1") +
                ",  x = " + point.value.ToString("F1");
        }
    }

    public void StopShowingCoordinate()
    {
        if (coordinateText != null)
            coordinateText.gameObject.SetActive(false);
    }

    public bool Matches(
        List<ContinuousPoint> target,
        float tolerance = 0.3f
    )
    {
        if (points.Count != target.Count)
            return false;

        for (int i = 0; i < points.Count; i++)
        {
            bool timeMatches =
                Mathf.Abs(points[i].t - target[i].t) <= tolerance;

            bool valueMatches =
                Mathf.Abs(points[i].value - target[i].value) <= tolerance;

            if (!timeMatches || !valueMatches)
                return false;
        }

        return true;
    }
}