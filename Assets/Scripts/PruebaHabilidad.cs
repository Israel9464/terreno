using UnityEngine;
using UnityEngine.SceneManagement;

public class PruebaHabilidad : MonoBehaviour
{
    public string escenaDerrota = "Derrota";
    
    [Header("Elementos de la UI")]
    public GameObject panelPrueba;      // El panel completo de la prueba
    public RectTransform barraFondo;    // La barra oscura de fondo
    public RectTransform zonaExito;     // La franja verde donde debes presionar E
    public RectTransform aguja;         // La línea blanca que se mueve

    [Header("Dificultad")]
    public float velocidadAguja = 1.25f; // Qué tan rápido cruza la barra (más alto = más difícil)

    private Perseguidor enemigoActual;
    private bool pruebaActiva = false;
    private float progreso = 0f;
    private float centroZona = 0.5f;
    private float margenZona = 0.1f;

    void Start()
    {
        if (panelPrueba != null) panelPrueba.SetActive(false);
    }

    public void IniciarPrueba(Perseguidor enemigo)
    {
        if (pruebaActiva) return;

        enemigoActual = enemigo;
        pruebaActiva = true;
        progreso = 0f;

        // Congela el juego mientras haces la prueba
        Time.timeScale = 0f;
        panelPrueba.SetActive(true);

        // Asegura que la zona y la aguja usen el centro de la barra como referencia
        zonaExito.anchorMin = zonaExito.anchorMax = zonaExito.pivot = new Vector2(0.5f, 0.5f);
        aguja.anchorMin = aguja.anchorMax = aguja.pivot = new Vector2(0.5f, 0.5f);

        // Coloca la zona verde en una posición aleatoria cada vez
        float anchoBarra = barraFondo.rect.width;
        float anchoZona = zonaExito.rect.width;
        margenZona = (anchoZona / anchoBarra) * 0.5f;

        centroZona = Random.Range(0.35f + margenZona, 0.85f - margenZona);
        float posXിക്ക = Mathf.Lerp(-anchoBarra * 0.5f, anchoBarra * 0.5f, centroZona);
        zonaExito.anchoredPosition = new Vector2(posXിക്ക, 0f);
        aguja.anchoredPosition = new Vector2(-anchoBarra * 0.5f, 0f);
    }

    void Update()
    {
        if (!pruebaActiva) return;

        // Mueve la aguja usando tiempo real (ignora el Time.timeScale = 0)
        progreso += velocidadAguja * Time.unscaledDeltaTime;

        float anchoBarra = barraFondo.rect.width;
        float posAgujaX = Mathf.Lerp(-anchoBarra * 0.5f, anchoBarra * 0.5f, progreso);
        aguja.anchoredPosition = new Vector2(posAgujaX, 0f);

        // Revisa si presionaste E (con un mínimo margen inicial para evitar clics accidentales)
        if (progreso > 0.05f && Input.GetKeyDown(KeyCode.E))
        {
            // Comprueba si la aguja está dentro de la zona verde
            if (Mathf.Abs(progreso - centroZona) <= margenZona + 0.02f)
            {
                Exito();
            }
            else
            {
                Fallo();
            }
            return;
        }

        // Si la aguja llegó al final sin que presionaras E, pierdes
        if (progreso >= 1f)
        {
            Fallo();
        }
    }

    void Exito()
    {
        pruebaActiva = false;
        panelPrueba.SetActive(false);
        Time.timeScale = 1f;

        if (enemigoActual != null)
        {
            enemigoActual.Aturdir(1.5f);
        }
    }

    void Fallo()
    {
        pruebaActiva = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaDerrota);
    }
}
