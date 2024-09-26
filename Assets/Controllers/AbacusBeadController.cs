using UnityEngine;
using UnityEngine.EventSystems;

public class AbacusBeadController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;
    private AbacusLineController lineController;

    void Start()
    {
        lineController = GetComponentInParent<AbacusLineController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineController.OnBeginDrag(index);
    }

    public void OnDrag(PointerEventData eventData)
    {
        lineController.OnDrag(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lineController.OnEndDrag();
    }
}