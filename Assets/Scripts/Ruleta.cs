using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Ruleta : MonoBehaviour
{
    public Button girarBoton;
    public Image ruletaImage;
    public float velocidadMinima = 150f;  // Velocidad mínima de giro
    public float velocidadMaxima = 300f;  // Velocidad máxima de giro
    public float tiempoGiro = 5f;
    private bool estaGirando = false;

    // Listas de escenas para cada dificultad
    public List<string> escenasFaciles = new List<string>();
    public List<string> escenasMedios = new List<string>();
    public List<string> escenasDificiles = new List<string>();

    void Start()
    {
        girarBoton.onClick.AddListener(GirarRuleta);
    }

    void GirarRuleta()
    {
        if (!estaGirando)
        {
            StartCoroutine(GirarRuletaCoroutine());
        }
    }

    IEnumerator GirarRuletaCoroutine()
    {
        estaGirando = true;

        // Selecciona una velocidad inicial aleatoria dentro del rango
        float velocidadGiro = Random.Range(velocidadMinima, velocidadMaxima);
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoGiro)
        {
            // Reducir progresivamente la velocidad
            float factorDeReduccion = 1f - (tiempoTranscurrido / tiempoGiro);
            float anguloRotacion = velocidadGiro * factorDeReduccion * Time.deltaTime;
            ruletaImage.transform.Rotate(0f, 0f, -anguloRotacion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Ajustar a un sector específico
        float anguloFinal = ruletaImage.transform.eulerAngles.z % 360;
        float sectorAncho = 360f / 6f; // 6 sectores en total (3 dificultades, con cada una apareciendo dos veces)
        float sectorObjetivo = Mathf.Round(anguloFinal / sectorAncho) * sectorAncho;
        ruletaImage.transform.rotation = Quaternion.Euler(0f, 0f, sectorObjetivo);

        // Determinar el sector seleccionado
        int sectorSeleccionado = Mathf.RoundToInt(anguloFinal / sectorAncho);
        string dificultadSeleccionada = "";

        // Ajustar los sectores para que los opuestos correspondan a la misma dificultad
        switch (sectorSeleccionado)
        {
            case 0: // Fácil
            case 3: // Fácil (opuesto)
                dificultadSeleccionada = "Fácil";
                StartCoroutine(CargarEscenaConRetraso(escenasFaciles, dificultadSeleccionada));
                break;
            case 1: // Medio
            case 4: // Medio (opuesto)
                dificultadSeleccionada = "Media";
                StartCoroutine(CargarEscenaConRetraso(escenasMedios, dificultadSeleccionada));
                break;
            case 2: // Difícil
            case 5: // Difícil (opuesto)
                dificultadSeleccionada = "Difícil";
                StartCoroutine(CargarEscenaConRetraso(escenasDificiles, dificultadSeleccionada));
                break;
        }

        estaGirando = false;
    }

    IEnumerator CargarEscenaConRetraso(List<string> listaEscenas, string dificultad)
    {
        if (listaEscenas.Count > 0)
        {
            string escenaSeleccionada = listaEscenas[Random.Range(0, listaEscenas.Count)];
            Debug.Log($"Dificultad seleccionada: {dificultad}, Escena seleccionada: {escenaSeleccionada}");

            // Espera de 2 segundos antes de cargar la escena
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(escenaSeleccionada);
        }
        else
        {
            Debug.LogWarning($"La lista de escenas para {dificultad} está vacía.");
        }
    }
}
