using UnityEngine;
using UnityEngine.SceneManagement;

public class Perseguidor : MonoBehaviour
{
    public Transform jugador;
    public PruebaHabilidad pruebaHabilidad;

    public float velocidad = 2.5f;        // Empieza lento
    public float aceleracion = 0.1f;      // Sube poco a poco
    public float velocidadMaxima = 6.0f;  // Tope para que no sea imposible

    [Header("Saltar / Pasar Obstáculos")]
    public float tiempoParaSaltar = 1.0f;
    public float alturaSalto = 1.8f;
    public float distanciaDeteccion = 1.1f;

    private Vector3 posicionSuelo;
    private float tiempoAtorado = 0f;
    private bool saltando = false;
    private float progresoSalto = 0f;
    private float tiempoIgnorarObstaculos = 0f;

    // Control de aturdimiento y única oportunidad
    private float tiempoAturdido = 0f;
    private bool oportunidadUsada = false;

    public string escenaDerrota = "Derrota";

    void Start()
    {
        posicionSuelo = transform.position;

        if (pruebaHabilidad == null)
        {
            pruebaHabilidad = FindFirstObjectByType<PruebaHabilidad>();
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Si superaste la prueba de habilidad, se queda quieto 1.5 segundos
        if (tiempoAturdido > 0f)
        {
            tiempoAturdido -= Time.deltaTime;
            return;
        }

        // Aumenta la velocidad, pero sin pasar nunca de velocidadMaxima
        velocidad = Mathf.Min(velocidad + aceleracion * Time.deltaTime, velocidadMaxima);

        if (tiempoIgnorarObstaculos > 0f)
        {
            tiempoIgnorarObstaculos -= Time.deltaTime;
        }

        Vector3 direccion = jugador.position - posicionSuelo;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.001f)
        {
            direccion.Normalize();
            transform.rotation = Quaternion.LookRotation(direccion);
        }

        if (saltando)
        {
            progresoSalto += Time.deltaTime / 0.7f;
            posicionSuelo = Vector3.MoveTowards(posicionSuelo, jugador.position, velocidad * 1.25f * Time.deltaTime);

            float arcoY = Mathf.Sin(Mathf.Clamp01(progresoSalto) * Mathf.PI) * alturaSalto;
            transform.position = posicionSuelo + Vector3.up * arcoY;

            if (progresoSalto >= 1f)
            {
                saltando = false;
                transform.position = posicionSuelo;
            }
            return;
        }

        bool hayObstaculo = tiempoIgnorarObstaculos <= 0f && HayObstaculoEnfrente(direccion);

        if (hayObstaculo)
        {
            Vector3 lado = Vector3.Cross(Vector3.up, direccion);
            if (!HayObstaculoEnfrente(lado))
            {
                posicionSuelo += lado * (velocidad * 0.5f * Time.deltaTime);
            }

            tiempoAtorado += Time.deltaTime;

            if (tiempoAtorado >= tiempoParaSaltar)
            {
                tiempoAtorado = 0f;
                saltando = true;
                progresoSalto = 0f;
                tiempoIgnorarObstaculos = 1.2f;
            }
        }
        else
        {
            tiempoAtorado = 0f;
            posicionSuelo = Vector3.MoveTowards(posicionSuelo, jugador.position, velocidad * Time.deltaTime);
        }

        transform.position = posicionSuelo;
    }

    public void Aturdir(float segundos)
    {
        tiempoAturdido = segundos;
        saltando = false;
        tiempoAtorado = 0f;
        transform.position = posicionSuelo;
    }

    bool HayObstaculoEnfrente(Vector3 dir)
    {
        Vector3 origen = posicionSuelo + Vector3.up * 0.5f;
        RaycastHit[] impactos = Physics.SphereCastAll(origen, 0.35f, dir, distanciaDeteccion, ~0, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in impactos)
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform)) continue;
            if (hit.collider.CompareTag("Player")) continue;

            return true;
        }
        return false;
    }

    void OnTriggerStay(Collider otro)
    {
        // Ignora colisiones mientras está aturdido o mientras la prueba está en pantalla
        if (tiempoAturdido > 0f || Time.timeScale == 0f) return;

        if (otro.CompareTag("Player"))
        {
            if (!oportunidadUsada && pruebaHabilidad != null)
            {
                oportunidadUsada = true;
                pruebaHabilidad.IniciarPrueba(this);
            }
            else
            {
                SceneManager.LoadScene(escenaDerrota);
            }
        }
    }
}