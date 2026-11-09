using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinuousGraphHandle : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private EditableSignalGraph graph;
    private int pointIndex;
    private bool canDrag;

    public void Setup(EditableSignalGraph owner, int index, bool draggable)
    {
        graph = owner;
        pointIndex = index;
        canDrag = draggable;

        GetComponent<Image>().raycastTarget = draggable;
    }

    public void SetPosition(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
            graph.MovePointFromPointer(pointIndex, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
            graph.MovePointFromPointer(pointIndex, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
            graph.StopShowingCoordinate();
    }
}