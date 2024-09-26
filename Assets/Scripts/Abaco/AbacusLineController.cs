using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AbacusLineController : MonoBehaviour
{
    [System.Serializable]
    public class AbacusBead
    {
        public RectTransform beadTransform;
        public int value = 1;
    }

    public List<AbacusBead> beads = new List<AbacusBead>();
    public float leftBoundary = 0f;
    public float rightBoundary = 100f;
    public float beadSpacing = 10f;
    public float activeThreshold = 50f; // Posición X a partir de la cual las cuentas se consideran activas

    private AbacusBead draggedBead;
    private float minX, maxX;

    void Start()
    {
        UpdateBeadBoundaries();
    }

    void UpdateBeadBoundaries()
    {
        for (int i = 0; i < beads.Count; i++)
        {
            float leftLimit = leftBoundary + i * beadSpacing;
            float rightLimit = rightBoundary - (beads.Count - 1 - i) * beadSpacing;
            beads[i].beadTransform.anchoredPosition = new Vector2(leftLimit, beads[i].beadTransform.anchoredPosition.y);
        }
    }

    public void OnBeginDrag(int index)
    {
        draggedBead = beads[index];
        UpdateDragLimits(index);
    }

    public void OnDrag(Vector2 delta)
    {
        if (draggedBead != null)
        {
            Vector2 newPos = draggedBead.beadTransform.anchoredPosition + new Vector2(delta.x, 0);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            draggedBead.beadTransform.anchoredPosition = newPos;
        }
    }

    public void OnEndDrag()
    {
        draggedBead = null;
    }

    void UpdateDragLimits(int index)
    {
        minX = index > 0 ? beads[index - 1].beadTransform.anchoredPosition.x + beadSpacing : leftBoundary;
        maxX = index < beads.Count - 1 ? beads[index + 1].beadTransform.anchoredPosition.x - beadSpacing : rightBoundary;
    }

    public int CalculateValue()
    {
        int total = 0;
        foreach (var bead in beads)
        {
            if (bead.beadTransform.anchoredPosition.x > activeThreshold)
            {
                total += bead.value;
            }
        }
        return total;
    }
}