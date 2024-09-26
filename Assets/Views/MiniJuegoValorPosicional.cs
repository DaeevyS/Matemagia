using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinijuegoValorPosicional : MonoBehaviour
{
    [SerializeField] private Text textoNumero;
    [SerializeField] private Button[] botonesRespuesta;
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

    private void Start()
    {
        botonNuevoNumero.onClick.AddListener(GenerarNuevoNumero);
        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            int index = i;
            botonesRespuesta[i].onClick.AddListener(() => VerificarRespuesta(index));
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
        ActualizarBotonesRespuesta();
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
                numeroFormateado += $"<color=#{ColorUtility.ToHtmlStringRGB(Color.black)}>{numeroTexto[i]}</color>";
            }
        }
        textoNumero.text = numeroFormateado;
    }

    private void ActualizarBotonesRespuesta()
    {
        int valorPosicionalCorrecto = GetValorPosicionalCorrecto();
        int cantidadBotones = incluirDecenasDeMil ? 5 : (incluirUnidadesDeMil ? 4 : 3);

        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            if (i < cantidadBotones)
            {
                botonesRespuesta[i].gameObject.SetActive(true);
                botonesRespuesta[i].GetComponentInChildren<Text>().text = GetOpcionRespuesta(i, valorPosicionalCorrecto);
            }
            else
            {
                botonesRespuesta[i].gameObject.SetActive(false);
            }
        }
    }

    private int GetValorPosicionalCorrecto()
    {
        int longitud = numeroActual.ToString().Length;
        return (int)Mathf.Pow(10, longitud - 1 - digitoResaltado);
    }

    private string GetOpcionRespuesta(int indiceBoton, int valorPosicionalCorrecto)
    {
        int opcion = valorPosicionalCorrecto * (int)Mathf.Pow(10, indiceBoton);
        return opcion.ToString();
    }

    private void VerificarRespuesta(int indiceRespuestaSeleccionada)
    {
        int valorPosicionalCorrecto = GetValorPosicionalCorrecto();
        if (int.Parse(botonesRespuesta[indiceRespuestaSeleccionada].GetComponentInChildren<Text>().text) == valorPosicionalCorrecto)
        {
            textoResultado.text = "¡Correcto! Has identificado el valor posicional correctamente.";
            textoResultado.color = Color.green;
        }
        else
        {
            textoResultado.text = $"Incorrecto. El valor posicional correcto era {valorPosicionalCorrecto}.";
            textoResultado.color = Color.red;
        }
    }
}
