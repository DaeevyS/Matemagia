using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HorizontalMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float boundaryLeft = 0f;
    public float boundaryRight = 100f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Comenzó el arrastre");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localPoint))
        {
            float newX = Mathf.Clamp(localPoint.x, boundaryLeft, boundaryRight);
            rectTransform.anchoredPosition = new Vector2(newX, originalPosition.y);
            Debug.Log("Moviendo a: " + rectTransform.anchoredPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Terminó el arrastre");
    }
}