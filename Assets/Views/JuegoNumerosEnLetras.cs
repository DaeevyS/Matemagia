using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class JuegoNumerosEnLetras : MonoBehaviour
{
    [SerializeField] private Text textoNumeroEnLetras;
    [SerializeField] private InputField campoRespuesta;
    [SerializeField] private Button botonVerificar;
    [SerializeField] private Button botonNuevoNumero;
    [SerializeField] private Text textoResultado;

    [Header("Configuración de Rango")]
    [SerializeField] private bool usarRangoHasta9999 = false;
    [SerializeField] private bool usarRangoHasta99999 = false;

    private int numeroActual;
    private string[] unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
    private string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
    private string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };
    private string[] especiales = { "diez", "once", "doce", "trece", "catorce", "quince" };

    private void Start()
    {
        botonVerificar.onClick.AddListener(VerificarRespuesta);
        botonNuevoNumero.onClick.AddListener(GenerarNuevoNumero);

        GenerarNuevoNumero();
    }

    private void GenerarNuevoNumero()
    {
        int maximo = 999;
        if (usarRangoHasta99999) maximo = 99999;
        else if (usarRangoHasta9999) maximo = 9999;

        numeroActual = Random.Range(1, maximo + 1);
        MostrarNumeroEnLetras();
        campoRespuesta.text = "";
        textoResultado.text = "";
    }

    private void MostrarNumeroEnLetras()
    {
        string numeroEnLetras = ConvertirNumeroALetras(numeroActual);
        textoNumeroEnLetras.text = ColorearNumero(numeroEnLetras);
    }

    private string ConvertirNumeroALetras(int numero)
    {
        if (numero == 0) return "cero";

        List<string> partes = new List<string>();

        if (numero >= 10000)
        {
            partes.Add(decenas[numero / 10000] + " mil");
            numero %= 10000;
        }

        if (numero >= 1000)
        {
            if (numero / 1000 == 1)
                partes.Add("mil");
            else
                partes.Add(unidades[numero / 1000] + " mil");
            numero %= 1000;
        }

        if (numero >= 100)
        {
            if (numero == 100)
                partes.Add("cien");
            else
                partes.Add(centenas[numero / 100]);
            numero %= 100;
        }

        if (numero >= 10 && numero <= 15)
        {
            partes.Add(especiales[numero - 10]);
        }
        else if (numero >= 10)
        {
            string decena = decenas[numero / 10];
            string unidad = unidades[numero % 10];
            if (numero % 10 != 0)
                partes.Add(decena + " y " + unidad);
            else
                partes.Add(decena);
        }
        else if (numero > 0)
        {
            partes.Add(unidades[numero]);
        }

        return string.Join(" ", partes);
    }

    private string ColorearNumero(string texto)
    {
        string[] partes = texto.Split(' ');
        string[] colores = { "#FF0000", "#FFFF00", "#00FF00", "#0000FF", "#800080" };

        for (int i = 0; i < partes.Length; i++)
        {
            int colorIndex = Mathf.Min(i, colores.Length - 1);
            partes[i] = $"<color={colores[colorIndex]}>{partes[i]}</color>";
        }

        return string.Join(" ", partes);
    }

    private void VerificarRespuesta()
    {
        if (int.TryParse(campoRespuesta.text, out int respuestaUsuario))
        {
            if (respuestaUsuario == numeroActual)
            {
                textoResultado.text = "¡Correcto! Has acertado el número.";
                textoResultado.color = Color.green;
            }
            else
            {
                textoResultado.text = $"Incorrecto. El número correcto era {numeroActual}.";
                textoResultado.color = Color.red;
            }
        }
        else
        {
            textoResultado.text = "Por favor, ingresa un número válido.";
            textoResultado.color = Color.yellow;
        }
    }
}
