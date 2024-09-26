using UnityEngine;
using System.Collections.Generic;

public class NumberGenerator : MonoBehaviour
{
    [SerializeField] private bool useRange1000to9999 = false;
    [SerializeField] private bool useRange10000to99999 = false;
    
    private int generatedNumber;
    private string numberInWords;

    private Dictionary<int, string> numberWords = new Dictionary<int, string>();

    void Start()
    {
        InitializeNumberWords(); // Inicializa el diccionario
    }

    private void InitializeNumberWords()
    {
        // Unidades
        string[] units = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        
        // Decenas
        string[] tens = { "", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

        // Decenas especiales
        string[] specialTens = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };

        // Centenas
        string[] hundreds = { "cien", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

        // Llenar el diccionario del 0 al 999
        for (int i = 0; i < 1000; i++)
        {
            if (i < 10) // Unidades
            {
                numberWords[i] = units[i];
            }
            else if (i < 20) // Decenas especiales
            {
                numberWords[i] = specialTens[i - 10];
            }
            else if (i < 100) // Decenas
            {
                int unit = i % 10;
                int ten = i / 10;
                numberWords[i] = tens[ten] + (unit > 0 ? " y " + units[unit] : "");
            }
            else // Centenas
            {
                int hundred = i / 100;
                int remainder = i % 100;
                numberWords[i] = (hundred == 1 ? hundreds[0] : hundreds[hundred - 1]) +
                                 (remainder > 0 ? " " + numberWords[remainder] : "");
            }
        }

        // Llenar miles y hasta 99,999
        for (int i = 1000; i < 100000; i++)
        {
            int thousand = i / 1000;
            int remainder = i % 1000;

            if (thousand == 1)
                numberWords[i] = "mil" + (remainder > 0 ? " " + numberWords[remainder] : "");
            else
                numberWords[i] = numberWords[thousand] + " mil" + (remainder > 0 ? " " + numberWords[remainder] : "");
        }
    }

    public void GenerateNumber()
    {
        int minRange = 1;
        int maxRange = 999;

        if (useRange10000to99999)
        {
            minRange = 10000;
            maxRange = 99999;
        }
        else if (useRange1000to9999)
        {
            minRange = 1000;
            maxRange = 9999;
        }

        generatedNumber = Random.Range(minRange, maxRange + 1);
        numberInWords = ConvertToWords(generatedNumber);
    }

    private string ConvertToWords(int number)
    {
        if (number < 30)
            return numberWords.ContainsKey(number) ? numberWords[number] : "";

        List<string> parts = new List<string>();

        if (number >= 1000)
        {
            int thousands = number / 1000;
            parts.Add(ConvertToWords(thousands) + " mil");
            number %= 1000;
        }

        if (number >= 100)
        {
            int hundreds = number / 100;
            if (hundreds == 1 && number != 100)
                parts.Add("ciento");
            else
                parts.Add(numberWords[hundreds * 100]);
            number %= 100;
        }

        if (number > 0)
        {
            if (number < 30)
                parts.Add(numberWords[number]);
            else
            {
                int tens = number / 10;
                int ones = number % 10;
                parts.Add(numberWords[tens * 10] + (ones > 0 ? " y " + numberWords[ones] : ""));
            }
        }

        return string.Join(" ", parts);
    }

    public int GetGeneratedNumber() => generatedNumber;
    public string GetNumberInWords() => numberInWords;
}
