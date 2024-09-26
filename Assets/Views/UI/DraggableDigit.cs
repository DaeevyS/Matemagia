using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableDigit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private DragAndDropManager manager;
    private Vector2 originalPosition;

    public void Initialize(DragAndDropManager manager, Canvas canvas)
    {
        this.manager = manager;
        this.canvas = canvas;
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        manager.DropDigit(gameObject, rectTransform.position);
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}
