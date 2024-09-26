using UnityEngine;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour
{
    [SerializeField] private GameObject[] digitPrefabs; // Asegúrate de que tienes los prefabs para los números del 0 al 9
    [SerializeField] public Transform[] dropSpots;
    [SerializeField] private Transform digitsContainer;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        InitializeDigits();
    }

    private void InitializeDigits()
    {
        for (int i = 0; i < digitPrefabs.Length; i++)
        {
            for (int j = 0; j < 5; j++) // Crea 5 instancias de cada prefab
            {
                GameObject digit = Instantiate(digitPrefabs[i], digitsContainer);
                DraggableDigit draggableDigit = digit.AddComponent<DraggableDigit>();
                draggableDigit.Initialize(this, canvas);
                digit.GetComponent<Image>().raycastTarget = true;

                // Cambiar el nombre del objeto para que sea único
                //digit.name = digitPrefabs[i].name + "_" + j; // Por ejemplo, "8_0", "8_1", etc.
            }
        }
    }

    public void DropDigit(GameObject digit, Vector2 position)
    {
        int closestSpotIndex = GetClosestSpotIndex(position);
        if (closestSpotIndex != -1)
        {
            digit.transform.SetParent(dropSpots[closestSpotIndex]);
            digit.transform.localPosition = Vector3.zero;
        }
        else
        {
            digit.transform.SetParent(digitsContainer);
            digit.transform.localPosition = Vector3.zero;
        }
    }

    private int GetClosestSpotIndex(Vector2 position)
    {
        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < dropSpots.Length; i++)
        {
            float distance = Vector2.Distance(position, dropSpots[i].position);
            if (distance < closestDistance && dropSpots[i].childCount == 0)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public int GetFormedNumber()
    {
        string numberString = "";
        foreach (Transform spot in dropSpots)
        {
            if (spot.childCount > 0)
            {
                // Obtiene el nombre del hijo y elimina el sufijo "(Clone)".
                string digitName = spot.GetChild(0).name.Replace("(Clone)", "").Trim();
                numberString += digitName; // Agrega el dígito a la cadena.
            }
            else
            {
                numberString += "0"; // Si no hay un dígito, añade un "0".
            }
        }

        // Verifica si numberString tiene caracteres válidos
        if (string.IsNullOrEmpty(numberString) || !IsDigitsOnly(numberString))
        {
            Debug.LogError("Formed number string is invalid: " + numberString);
            return 0; // O manejar de otra manera según tu lógica.
        }

        return int.Parse(numberString);
    }

    // Método auxiliar para verificar si la cadena contiene solo dígitos
    private bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }
}

