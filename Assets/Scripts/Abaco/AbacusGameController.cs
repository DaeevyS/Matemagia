using UnityEngine;
using UnityEngine.UI;

public class AbacusGameController : MonoBehaviour
{
    public AbacusLineController unitsLine;
    public AbacusLineController tensLine;
    public AbacusLineController hundredsLine;
    public AbacusLineController uMilLine;
    public AbacusLineController dMilLine;
    public Text targetNumberText;
    public Text currentValueText;

    public GameObject unidadDeMil;
    public GameObject decenaDeMil;

    private int targetNumber;

    void Start()
    {
        GenerateNewNumber();
    }

    void Update()
    {
        int currentValue = unitsLine.CalculateValue() + 
                           tensLine.CalculateValue() * 10 + 
                           hundredsLine.CalculateValue() * 100 +
                           uMilLine.CalculateValue() * 1000 +
                           dMilLine.CalculateValue() * 10000;
        currentValueText.text = "Current Value: " + currentValue;

        if (currentValue == targetNumber)
        {
            Debug.Log("¡Correcto! Generando nuevo número...");
            GenerateNewNumber();
        }
    }

    void GenerateNewNumber()
    {
        // Si Unidad de Mil está encendida y Decena de Mil está apagada
        if (unidadDeMil.activeSelf && !decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(1001, 9999);
        }
        // Si Unidad de Mil está apagada y Decena de Mil está encendida
        else if (unidadDeMil.activeSelf && decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(10000, 100000);
        }
        // Si ambos están apagados (caso por defecto)
        else if (!unidadDeMil.activeSelf && !decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(0, 1000);
        }
        
        // Actualiza el texto con el nuevo número generado
        targetNumberText.text = "Target Number: " + targetNumber;
    }
}