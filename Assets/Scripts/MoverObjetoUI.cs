using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Necesario para manejar los eventos de arrastre

public class MoverObjetoUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform limiteIzquierdo;
    public RectTransform limiteDerecho;
    public List<RectTransform> objetosEnLinea; // Los RectTransforms de las imágenes en línea
    public float espacioEntreObjetos = 10f; // Espacio mínimo entre los objetos

    private RectTransform objetoArrastrado; // El objeto que estamos arrastrando
    private Vector3 posicionInicial;

    private void Start()
    {
        // Validación para evitar errores de referencia
        if (limiteIzquierdo == null || limiteDerecho == null)
        {
            Debug.LogError("Los límites no están asignados.");
            return;
        }

        if (objetosEnLinea == null || objetosEnLinea.Count == 0)
        {
            Debug.LogWarning("No se han asignado objetos en línea, buscando hijos con RectTransform...");
            objetosEnLinea = new List<RectTransform>();

            foreach (Transform child in transform) 
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    objetosEnLinea.Add(rectTransform); // Añadimos el RectTransform de cada hijo
                }
                else
                {
                    Debug.LogWarning($"{child.name} no tiene RectTransform y será omitido.");
                }
            }

            if (objetosEnLinea.Count == 0)
            {
                Debug.LogError("No se encontraron hijos con RectTransform.");
            }
        }
    }

    // Detecta cuando comienza el arrastre
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Verificamos si el objeto que arrastramos está en la lista
        objetoArrastrado = eventData.pointerDrag.GetComponent<RectTransform>();

        if (objetoArrastrado != null && objetosEnLinea.Contains(objetoArrastrado))
        {
            posicionInicial = objetoArrastrado.localPosition;
        }
    }

    // Maneja el arrastre
    public void OnDrag(PointerEventData eventData)
    {
        if (objetoArrastrado != null)
        {
            // Movemos el objeto con el toque
            Vector3 posicion = objetoArrastrado.localPosition + new Vector3(eventData.delta.x, 0, 0);

            // Restringimos el movimiento a los límites
            posicion.x = Mathf.Clamp(posicion.x, limiteIzquierdo.localPosition.x, limiteDerecho.localPosition.x);

            // Revisamos que no se sobrepongan con otros objetos
            for (int i = 0; i < objetosEnLinea.Count; i++)
            {
                if (objetosEnLinea[i] != objetoArrastrado)
                {
                    // Evitar la superposición con el objeto a la izquierda
                    if (posicion.x < objetosEnLinea[i].localPosition.x + espacioEntreObjetos &&
                        objetoArrastrado.localPosition.x > objetosEnLinea[i].localPosition.x)
                    {
                        posicion.x = objetosEnLinea[i].localPosition.x + espacioEntreObjetos;
                    }

                    // Evitar la superposición con el objeto a la derecha
                    if (posicion.x > objetosEnLinea[i].localPosition.x - espacioEntreObjetos &&
                        objetoArrastrado.localPosition.x < objetosEnLinea[i].localPosition.x)
                    {
                        posicion.x = objetosEnLinea[i].localPosition.x - espacioEntreObjetos;
                    }
                }
            }

            // Aplicamos la nueva posición
            objetoArrastrado.localPosition = posicion;
        }
    }

    // Detecta cuando termina el arrastre
    public void OnEndDrag(PointerEventData eventData)
    {
        objetoArrastrado = null; // Ya no estamos arrastrando ningún objeto
    }
}
