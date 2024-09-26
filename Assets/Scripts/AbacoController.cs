using UnityEngine;
using UnityEngine.UI;

public class AbacoController : MonoBehaviour
{
    public GameObject[] unidades = new GameObject[10];
    public GameObject[] decenas = new GameObject[10];
    public GameObject[] centenas = new GameObject[10];

    public Sprite spriteCentenas; // Sprite para las centenas (Ej: nueces grandes)
    public Sprite spriteDecenas;  // Sprite para las decenas (Ej: nueces medianas)
    public Sprite spriteUnidades; // Sprite para las unidades (Ej: nueces pequeñas)

    public Text numeroObjetivoText;  // Texto del número objetivo en UI
    public Text numeroActualText;    // Texto del número actual en UI

    private int numeroObjetivo;
    private int valorUnidades = 0;
    private int valorDecenas = 0;
    private int valorCentenas = 0;

    private float screenWidthUnits;
    private float screenHeightUnits;
    private float radioBolita;

    void Start()
    {
        // Obtener el tamaño de la pantalla en unidades del mundo (basado en la cámara)
        screenHeightUnits = Camera.main.orthographicSize * 2f;
        screenWidthUnits = screenHeightUnits * Screen.width / Screen.height;

        // Definir el tamaño del espacio entre bolitas (ajustado según el tamaño de la pantalla)
        float espacioEntreBolitas = screenWidthUnits / 12f; // Ajustamos el espacio para que haya un buen margen
        
        // Obtener el tamaño del sprite de la bolita (en este caso, usaré el sprite de centenas como ejemplo)
        float bolitaWidth = spriteCentenas.bounds.size.x;
        float bolitaHeight = spriteCentenas.bounds.size.y;
        radioBolita = bolitaWidth / 2;  // Definir el radio de la bolita

        // Calcular la posición vertical central de las filas
        float yCentral = screenHeightUnits / 2f;

        // Ajustar las posiciones de cada fila (unidades, decenas, centenas) para que estén centradas
        Vector2 centroUnidades = new Vector2(0, yCentral - bolitaHeight);
        Vector2 centroDecenas = new Vector2(0, yCentral);
        Vector2 centroCentenas = new Vector2(0, yCentral + bolitaHeight);

        // Crear las bolitas y centrarlas
        CrearBolitas(unidades, centroUnidades, espacioEntreBolitas);
        CrearBolitas(decenas, centroDecenas, espacioEntreBolitas);
        CrearBolitas(centenas, centroCentenas, espacioEntreBolitas);

        GenerarNumeroAleatorio(); // Generar el número objetivo al inicio
    }

    // Renombramos el método CrearBolitas sin parámetros a CrearTodasLasBolitas
    void CrearTodasLasBolitas()
    {
        float screenHeightUnits = Camera.main.orthographicSize * 2;
        float screenWidthUnits = screenHeightUnits * Screen.width / Screen.height;
        float espacioEntreBolitas = screenWidthUnits / 12;

        float posicionYCentenas = screenHeightUnits * 0.2f;
        float posicionYDecenas = screenHeightUnits * 0.1f;
        float posicionYUnidades = 0f;

        CrearFilaBolitas(centenas, spriteCentenas, new Vector2(0, posicionYCentenas), espacioEntreBolitas);
        CrearFilaBolitas(decenas, spriteDecenas, new Vector2(0, posicionYDecenas), espacioEntreBolitas);
        CrearFilaBolitas(unidades, spriteUnidades, new Vector2(0, posicionYUnidades), espacioEntreBolitas);
    }

    // Crear bolitas con los argumentos requeridos (array de bolitas, posición inicial, espacio entre bolitas)
    void CrearBolitas(GameObject[] bolitas, Vector2 posicionInicial, float espacioEntreBolitas)
    {
        float inicioX = posicionInicial.x - (espacioEntreBolitas * (bolitas.Length - 1)) / 2;

        for (int i = 0; i < bolitas.Length; i++)
        {
            Vector3 nuevaPosicion = new Vector3(inicioX + i * espacioEntreBolitas, posicionInicial.y, 0);
            GameObject nuevaBolita = new GameObject("Bolita" + i);
            SpriteRenderer renderer = nuevaBolita.AddComponent<SpriteRenderer>();
            renderer.sprite = spriteUnidades;  // Puedes cambiar el sprite según corresponda (unidades, decenas, centenas)
            nuevaBolita.transform.position = nuevaPosicion;
            nuevaBolita.transform.SetParent(transform);  // Opcional, para organizarlas bajo el mismo objeto
            bolitas[i] = nuevaBolita;
        }
    }

    void CrearFilaBolitas(GameObject[] bolitas, Sprite sprite, Vector2 posicionInicial, float espacioEntreBolitas)
    {
        float inicioX = posicionInicial.x - (espacioEntreBolitas * (bolitas.Length - 1)) / 2;

        for (int i = 0; i < bolitas.Length; i++)
        {
            Vector3 nuevaPosicion = new Vector3(inicioX + i * espacioEntreBolitas, posicionInicial.y, 0);
            GameObject nuevaBolita = new GameObject("Bolita" + i);
            SpriteRenderer renderer = nuevaBolita.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            nuevaBolita.transform.position = nuevaPosicion;
            nuevaBolita.transform.SetParent(transform);  // Opcional, para organizarlas bajo el mismo objeto
            bolitas[i] = nuevaBolita;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;  // Mantener el eje Z en 0 para 2D

            if (touch.phase == TouchPhase.Moved)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    GameObject seleccionada = hit.transform.gameObject;
                    
                    // Pasa el array correcto de bolitas (unidades, decenas, centenas) según corresponda
                    MoverBolita(seleccionada, touchPosition, unidades);  
                    CalcularValorAbaco(); // Recalcular el valor total al mover una bolita
                }
            }
        }
    }

    void MoverBolita(GameObject bolita, Vector3 nuevaPosicion, GameObject[] bolitas)
    {
        // Restricción horizontal (dentro de los límites de la pantalla)
        float minX = -screenWidthUnits / 2 + radioBolita;
        float maxX = screenWidthUnits / 2 - radioBolita;

        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, minX, maxX);

        // Mover la bolita solo si no colisiona con otras
        if (!ColisionaConOtraBolita(bolita, nuevaPosicion, bolitas))
        {
            bolita.transform.position = nuevaPosicion;
        }
    }

    bool ColisionaConOtraBolita(GameObject bolita, Vector3 nuevaPosicion, GameObject[] bolitas)
    {
        foreach (var otraBolita in bolitas)
        {
            if (otraBolita != bolita)
            {
                float distancia = Vector3.Distance(nuevaPosicion, otraBolita.transform.position);
                if (distancia < radioBolita * 2)
                {
                    return true;  // Colisión detectada
                }
            }
        }
        return false;
    }

    void CalcularValorAbaco()
    {
        valorUnidades = 0;
        valorDecenas = 0;
        valorCentenas = 0;

        // Contar bolitas en la fila de unidades
        foreach (GameObject bolita in unidades)
        {
            if (bolita.transform.position.x > 0)
            {
                valorUnidades++;
            }
        }

        // Contar bolitas en la fila de decenas
        foreach (GameObject bolita in decenas)
        {
            if (bolita.transform.position.x > 0)
            {
                valorDecenas++;
            }
        }

        // Contar bolitas en la fila de centenas
        foreach (GameObject bolita in centenas)
        {
            if (bolita.transform.position.x > 0)
            {
                valorCentenas++;
            }
        }

        int valorTotal = valorUnidades + (valorDecenas * 10) + (valorCentenas * 100);
        ActualizarNumeroActual(valorTotal);  // Actualizar el texto del valor actual

        // Mostrar el número actual en la consola
        Debug.Log("Número Actual: " + valorTotal);
    }

    void GenerarNumeroAleatorio()
    {
        numeroObjetivo = Random.Range(0, 1000);  // Generar un número aleatorio entre 0 y 999
        numeroObjetivoText.text = "Número Objetivo: " + numeroObjetivo.ToString();

        // Mostrar el número objetivo en la consola
        Debug.Log("Número Objetivo: " + numeroObjetivo);
    }

    void ActualizarNumeroActual(int valorTotal)
    {
        numeroActualText.text = "Número Actual: " + valorTotal.ToString();

        if (valorTotal == numeroObjetivo)
        {
            Debug.Log("¡Felicidades! Has acertado el número.");
        }
    }
}

