using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class JuegoValorPosicional : MonoBehaviour
{
    [SerializeField] private Text textoNumero;
    [SerializeField] private Button[] botonesValorPosicional;
    [SerializeField] private Text textoResultado;
    [SerializeField] private Button botonNuevoNumero;

    [Header("Configuración de Rango")]
    [SerializeField] private bool incluirUnidadesDeMil = false;
    [SerializeField] private bool incluirDecenasDeMil = false;

    [Header("Configuración de Resaltado")]
    [SerializeField] private Color colorResaltado = Color.yellow;
    [SerializeField] private float tamanoResaltado = 1.5f;

    private int numeroActual;
    private int digitoResaltado;
    private string[] nombresValoresPosicionales = { "Unidad", "Decena", "Centena", "Unidad de Mil", "Decena de Mil" };
    private Color[] coloresDigitos = { Color.red, Color.blue, Color.green, Color.magenta, Color.cyan };

    private void Start()
    {
        botonNuevoNumero.onClick.AddListener(GenerarNuevoNumero);
        for (int i = 0; i < botonesValorPosicional.Length; i++)
        {
            int index = i;
            botonesValorPosicional[i].onClick.AddListener(() => VerificarRespuesta(index));
        }
        GenerarNuevoNumero();
    }

    private void GenerarNuevoNumero()
    {
        int maximo = 999;
        if (incluirDecenasDeMil) maximo = 99999;
        else if (incluirUnidadesDeMil) maximo = 9999;
        numeroActual = Random.Range(1, maximo + 1);
        
        int longitud = numeroActual.ToString().Length;
        digitoResaltado = Random.Range(0, longitud);
        
        MostrarNumero();
        ActualizarBotonesValorPosicional();
        textoResultado.text = "";
    }

    private void MostrarNumero()
    {
        string numeroTexto = numeroActual.ToString();
        string numeroFormateado = "";
        
        for (int i = 0; i < numeroTexto.Length; i++)
        {
            if (i == digitoResaltado)
            {
                numeroFormateado += $"<color=#{ColorUtility.ToHtmlStringRGB(colorResaltado)}><size={100 * tamanoResaltado}%><b>{numeroTexto[i]}</b></size></color>";
            }
            else
            {
                numeroFormateado += $"<color=#{ColorUtility.ToHtmlStringRGB(coloresDigitos[i])}>{numeroTexto[i]}</color>";
            }
        }
        textoNumero.text = numeroFormateado;
    }

    private void ActualizarBotonesValorPosicional()
    {
        int cantidadBotones = incluirDecenasDeMil ? 5 : (incluirUnidadesDeMil ? 4 : 3);
        for (int i = 0; i < botonesValorPosicional.Length; i++)
        {
            if (i < cantidadBotones)
            {
                botonesValorPosicional[i].gameObject.SetActive(true);
                botonesValorPosicional[i].GetComponentInChildren<Text>().text = nombresValoresPosicionales[i];
            }
            else
            {
                botonesValorPosicional[i].gameObject.SetActive(false);
            }
        }
    }

    private void VerificarRespuesta(int valorPosicionalSeleccionado)
    {
        int valorPosicionalCorrecto = numeroActual.ToString().Length - 1 - digitoResaltado;
        if (valorPosicionalSeleccionado == valorPosicionalCorrecto)
        {
            textoResultado.text = "¡Correcto! Has identificado el valor posicional correctamente.";
            textoResultado.color = Color.green;
        }
        else
        {
            textoResultado.text = $"Incorrecto. El valor posicional correcto era {nombresValoresPosicionales[valorPosicionalCorrecto]}.";
            textoResultado.color = Color.red;
        }
    }
}